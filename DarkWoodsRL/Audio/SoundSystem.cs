using System;
using System.IO;
using System.Text;
using OpenTK.Audio.OpenAL;

// ReSharper disable MemberCanBePrivate.Local

namespace DarkWoodsRL.Audio;

public class SoundSystem
{
    private static readonly Encoding UTF8 = new UTF8Encoding();
    private static ALDevice _device;
    private static ALContext _context;


    public static void Init()
    {
        _device = ALC.OpenDevice(null);
        if (_device == ALDevice.Null)
        {
            throw new Exception("Error with creating device.");
        }

        var error = AL.GetError();
        if (error != ALError.NoError)
        {
            throw new Exception("Error when checking for an error ?");
        }

        _context = ALC.CreateContext(_device, Array.Empty<int>());
        if (!ALC.MakeContextCurrent(_context))
        {
            throw new Exception("Error with creating context");
        }

        AL.Listener(ALListenerf.Gain, 0.2f);
    }

    public static void Discard()
    {
        ALC.DestroyContext(_context);
        ALC.CloseDevice(_device);
        ALC.MakeContextCurrent(ALContext.Null);
    }

    public void Play(string sound)
    {
        var stream = new FileStream(sound, FileMode.Open);
        var wav = _readWav(stream);

        var format = wav.BitsPerSample switch
        {
            16 when wav.NumChannels == 1 => ALFormat.Mono16,
            16 when wav.NumChannels == 2 => ALFormat.Stereo16,
            16 => throw new InvalidOperationException("Unable to load audio with more than 2 channels."),
            8 when wav.NumChannels == 1 => ALFormat.Mono8,
            8 when wav.NumChannels == 2 => ALFormat.Stereo8,
            8 => throw new InvalidOperationException("Unable to load audio with more than 2 channels."),
            _ => throw new InvalidOperationException("Unable to load wav with bits per sample different from 8 or 16")
        };

        var buffer = AL.GenBuffer();

        unsafe
        {
            fixed (byte* ptr = wav.Data.Span)
            {
                AL.BufferData(buffer, format, (IntPtr) ptr, wav.Data.Length, wav.SampleRate);
            }
        }

        AL.GenSource(out var alSource);
        AL.Source(alSource, ALSourcef.Gain, 1f);
        AL.Source(alSource, ALSourcei.Buffer, buffer);
        AL.SourcePlay(alSource);
    }

    private static WavData _readWav(Stream stream)
    {
        var reader = new BinaryReader(stream, UTF8, true);

        void SkipChunk()
        {
            var length = reader.ReadUInt32();
            stream.Position += length;
        }

        // Read outer most chunks.
        Span<char> fourCc = stackalloc char[4];
        while (true)
        {
            _readFourCC(reader, fourCc);

            if (fourCc.SequenceEqual("RIFF")) return _readRiffChunk(reader);
            SkipChunk();
        }
    }

    private static void _skipChunk(BinaryReader reader)
    {
        var length = reader.ReadUInt32();
        reader.BaseStream.Position += length;
    }

    private static void _readFourCC(BinaryReader reader, Span<char> fourCc)
    {
        fourCc[0] = (char) reader.ReadByte();
        fourCc[1] = (char) reader.ReadByte();
        fourCc[2] = (char) reader.ReadByte();
        fourCc[3] = (char) reader.ReadByte();
    }

    private static WavData _readRiffChunk(BinaryReader reader)
    {
        Span<char> format = stackalloc char[4];
        reader.ReadUInt32();
        _readFourCC(reader, format);
        if (!format.SequenceEqual("WAVE"))
        {
            throw new InvalidDataException("File is not a WAVE file.");
        }

        _readFourCC(reader, format);
        if (!format.SequenceEqual("fmt "))
        {
            throw new InvalidDataException("Expected fmt chunk.");
        }

        // Read fmt chunk.

        var size = reader.ReadInt32();
        var afterFmtPos = reader.BaseStream.Position + size;

        var audioType = (WavAudioFormatType) reader.ReadInt16();
        var channels = reader.ReadInt16();
        var sampleRate = reader.ReadInt32();
        var byteRate = reader.ReadInt32();
        var blockAlign = reader.ReadInt16();
        var bitsPerSample = reader.ReadInt16();

        if (audioType != WavAudioFormatType.PCM)
        {
            throw new NotImplementedException("Unable to support audio types other than PCM.");
        }

        // Fmt is not of guaranteed size, so use the size header to skip to the end.
        reader.BaseStream.Position = afterFmtPos;

        while (true)
        {
            _readFourCC(reader, format);
            if (!format.SequenceEqual("data"))
            {
                _skipChunk(reader);
                continue;
            }

            break;
        }

        // We are in the data chunk.
        size = reader.ReadInt32();
        var data = reader.ReadBytes(size);

        return new WavData(audioType, channels, sampleRate, byteRate, blockAlign, bitsPerSample, data);
    }

    /// <summary>
    ///     See http://soundfile.sapp.org/doc/WaveFormat/ for reference.
    /// </summary>
    private readonly struct WavData
    {
        // ReSharper disable struct NotAccessedField.Local
        public readonly WavAudioFormatType AudioType;
        public readonly short NumChannels;
        public readonly int SampleRate;
        public readonly int ByteRate;
        public readonly short BlockAlign;
        public readonly short BitsPerSample;
        public readonly ReadOnlyMemory<byte> Data;

        public WavData(WavAudioFormatType audioType, short numChannels, int sampleRate, int byteRate,
            short blockAlign, short bitsPerSample, ReadOnlyMemory<byte> data)
        {
            AudioType = audioType;
            NumChannels = numChannels;
            SampleRate = sampleRate;
            ByteRate = byteRate;
            BlockAlign = blockAlign;
            BitsPerSample = bitsPerSample;
            Data = data;
        }
    }

    private enum WavAudioFormatType : short
    {
        PCM = 1
        // There's a bunch of other types, those are all unsupported.
    }
}