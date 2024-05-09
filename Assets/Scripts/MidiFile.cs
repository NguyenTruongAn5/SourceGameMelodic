using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Library
{
    public class MidiParser
    {
        public float TempoBPM;
        public List<Track> Tracks;

        public MidiParser(string path)
        {
            var midiFile = new MidiFile(path);
            detectTrack(midiFile);
            PrintNotes();
        }

        public MidiParser(Stream stream)
        {
            var midiFile = new MidiFile(stream);
            detectTrack(midiFile);
            PrintNotes();
        }

        public MidiParser(byte[] data)
        {
            var midiFile = new MidiFile(data);
            detectTrack(midiFile);
            PrintNotes();
        }

        private void DebugLog(string mes)
        {
            if (Application.isEditor)
            {
                Debug.Log(mes);
            }
        }
        private void PrintNotes()
        {
            if (Application.isEditor)
            {
                //DebugLog("Tracks: " + Newtonsoft.Json.JsonConvert.SerializeObject(Tracks, Newtonsoft.Json.Formatting.Indented));
                //DebugLog("Beats: " + Newtonsoft.Json.JsonConvert.SerializeObject(Beats, Newtonsoft.Json.Formatting.Indented));
            }
        }
        public float GetValueBarNote()
        {
            float QuarterNote = Mathf.Round(60000 / TempoBPM);
            float barNote = QuarterNote * 4;
            return barNote;
        }

        private void detectTrack(MidiFile midiFile)
        {
            if (midiFile != null)
            {
                TempoBPM = getTempo(midiFile);
                Tracks = getTracks(midiFile, TempoBPM);
                DebugLog($"TempoBPM: {TempoBPM} - Count track: {Tracks.Count}");
            }
        }

        private float getTempo(MidiFile midiFile)
        {
            int tempoBPM = 120;
            if (midiFile.Tracks != null)
            {
                foreach (var track in midiFile.Tracks)
                {
                    foreach (var midiEvent in track.MidiEvents)
                    {
                        //Get tempo bpm
                        if (midiEvent.MetaEventType == MetaEventType.Tempo && midiEvent.Time == 0)
                        {
                            return midiEvent.Arg2;
                        }
                    }
                }
            }
            return tempoBPM;
        }

        private List<Track> getTracks(MidiFile midiFile, float tempoBPM)
        {
            List<Track> tracks = new List<Track>();
            if (midiFile.Tracks != null)
            {
                //Get ratio time
                int timebase = midiFile.TicksPerQuarterNote;
                double tempoMS = 60000000 / tempoBPM;
                double ratioTime = (tempoMS / 1000) / timebase;
                //DebugLog($"TempoBPM {tempoBPM} TempoMS {tempoMS} Timebase {timebase} ratioTime {ratioTime}");

                //Get track has noteon
                List<MidiTrack> newtracks = new List<MidiTrack>();
                foreach (var track in midiFile.Tracks)
                {
                    string trackName = null;
                    foreach (var textEvent in track.TextEvents)
                    {
                        if (textEvent.TextEventType == TextEventType.TrackName)
                        {
                            trackName = textEvent.Value;
                            break;
                        }
                    }
                    foreach (var midiEvent in track.MidiEvents)
                    {
                        if (midiEvent.MidiEventType == MidiEventType.NoteOn)
                        {
                            newtracks.Add(new MidiTrack { TrackName = trackName, Channel = midiEvent.Channel, MidiEvents = track.MidiEvents, TextEvents = track.TextEvents });
                            break;
                        }
                    }
                }

                //Get beats note
                foreach (var track in newtracks)
                {
                    List<Beat> beatsTrack = new List<Beat>();
                    int indexNote = -1;
                    double prevCurrentTime = 0;
                    double deltaTime = 0;
                    double deltaTimeOff = 0;
                    void setTimeOff(int numberNote, double CurrentTime)
                    {
                        for (int i = beatsTrack.Count - 1; i >= 0; i--)
                        {
                            for (int j = 0; j < beatsTrack[i].Notes.Count; j++)
                            {
                                if (beatsTrack[i].Notes[j].Number == numberNote && beatsTrack[i].Notes[j].TimeOff == 0)
                                {
                                    beatsTrack[i].Notes[j].TimeOff = CurrentTime - beatsTrack[i].CurrentTime;
                                    return;
                                }
                            }
                        }
                    }

                    foreach (var midiEvent in track.MidiEvents)
                    {
                        if (midiEvent.MidiEventType == MidiEventType.NoteOn || midiEvent.MidiEventType == MidiEventType.NoteOff)
                        {
                            double time = midiEvent.Time * ratioTime;
                            double CurrentTime = midiEvent.CurrentTime * ratioTime;
                            var channel = midiEvent.Channel;
                            var numberNote = midiEvent.Note;
                            var velocity = midiEvent.Velocity;
                            if (midiEvent.MidiEventType == MidiEventType.NoteOn)
                            {
                                deltaTime = time + deltaTimeOff;
                                var note = new Note { Number = numberNote, Velocity = velocity, Channel = channel, TrackName = track.TrackName };
                                if (indexNote > -1 && (CurrentTime - prevCurrentTime) < 15)
                                {
                                    beatsTrack[indexNote].Notes.Add(note);
                                }
                                else
                                {
                                    indexNote = indexNote + 1;
                                    var beat = new Beat { CurrentTime = CurrentTime };
                                    beat.Notes.Add(note);
                                    beatsTrack.Add(beat);
                                }
                                prevCurrentTime = CurrentTime;
                                deltaTimeOff = 0;
                            }
                            else if (midiEvent.MidiEventType == MidiEventType.NoteOff)
                            {
                                deltaTimeOff = deltaTimeOff + time;
                                setTimeOff(numberNote, CurrentTime);
                            }
                        }
                    }
                    if (beatsTrack.Count > 0)
                    {
                        DebugLog($"track {tracks.Count} channel {track.Channel} trackname {track.TrackName} beats {beatsTrack.Count}");
                        tracks.Add(new Track { TrackName = track.TrackName, Beats = beatsTrack });
                    }
                }
            }
            return tracks;
        }
    }

    public class Track
    {
        public string TrackName;

        public int Channel;

        public List<Beat> Beats = new List<Beat>();

    }
    public class Beat
    {
        public int IndexNote;

        public List<Note> Notes = new List<Note>();

        public double CurrentTime;

        public double Time;
    }

    public class Note
    {
        public int Number;

        public int Velocity;

        public double TimeOff;

        public int Channel;

        public string TrackName;

        public bool LongNote;
    }

    public class MidiFile
    {
        public readonly int Format;

        public readonly int TicksPerQuarterNote;

        public readonly MidiTrack[] Tracks;

        public readonly int TracksCount;

        public MidiFile(Stream stream)
            : this(Reader.ReadAllBytesFromStream(stream))
        {
        }

        public MidiFile(string path, bool isEncrypt = false)
            : this(File.ReadAllBytes(path))
        {
        }

        public MidiFile(byte[] data)
        {
            var position = 0;

            if (Reader.ReadString(data, ref position, 4) != "MThd")
            {
                //throw new FormatException("Invalid file header (expected MThd)");
                return;
            }

            if (Reader.Read32(data, ref position) != 6)
            {
                //throw new FormatException("Invalid header length (expected 6)");
                return;
            }

            this.Format = Reader.Read16(data, ref position);
            this.TracksCount = Reader.Read16(data, ref position);
            this.TicksPerQuarterNote = Reader.Read16(data, ref position);

            if ((this.TicksPerQuarterNote & 0x8000) != 0)
            {
                //throw new FormatException("Invalid timing mode (SMPTE timecode not supported)");
                return;
            }

            this.Tracks = new MidiTrack[this.TracksCount];

            for (var i = 0; i < this.TracksCount; i++)
            {
                this.Tracks[i] = ParseTrack(i, data, ref position);
            }
        }

        private static bool ParseMetaEvent(
            byte[] data,
            ref int position,
            byte metaEventType,
            ref byte data1,
            ref byte data2)
        {
            switch (metaEventType)
            {
                case (byte)MetaEventType.Tempo:
                    var mspqn = (data[position + 1] << 16) | (data[position + 2] << 8) | data[position + 3];
                    data1 = (byte)Mathf.Round((60000000f / (float)mspqn));
                    position += 4;
                    return true;

                case (byte)MetaEventType.TimeSignature:
                    data1 = data[position + 1];
                    data2 = (byte)Math.Pow(2.0, data[position + 2]);
                    position += 5;
                    return true;

                case (byte)MetaEventType.KeySignature:
                    data1 = data[position + 1];
                    data2 = data[position + 2];
                    position += 3;
                    return true;

                // Ignore Other Meta Events
                default:
                    var length = Reader.ReadVarInt(data, ref position);
                    position += length;
                    return false;
            }
        }

        private static MidiTrack ParseTrack(int index, byte[] data, ref int position)
        {
            if (Reader.ReadString(data, ref position, 4) != "MTrk")
            {
                //throw new FormatException("Invalid track header (expected MTrk)");
                return null;
            }

            var trackLength = Reader.Read32(data, ref position);
            var trackEnd = position + trackLength;

            var track = new MidiTrack { Index = index };
            var CurrentTime = 0;
            var status = (byte)0;
            int difTimeMidiBank = 16384;

            while (position < trackEnd)
            {
                var time = Reader.ReadVarInt(data, ref position);
                if (time >= difTimeMidiBank)
                {
                    time = time - difTimeMidiBank;
                }
                CurrentTime += time;


                var peekByte = data[position];

                // If the most significant bit is set then this is a status byte
                if ((peekByte & 0x80) != 0)
                {
                    status = peekByte;
                    ++position;
                }

                // If the most significant nibble is not an 0xF this is a channel event
                if ((status & 0xF0) != 0xF0)
                {
                    // Separate event type from channel into two
                    var eventType = (byte)(status & 0xF0);
                    var channel = (byte)((status & 0x0F) + 1);

                    var data1 = data[position++];

                    // If the event type doesn't start with 0b110 it has two bytes of data (i.e. except 0xC0 and 0xD0)
                    var data2 = (eventType & 0xE0) != 0xC0 ? data[position++] : (byte)0;

                    // Convert NoteOn events with 0 velocity into NoteOff events
                    if (eventType == (byte)MidiEventType.NoteOn && data2 == 0)
                    {
                        eventType = (byte)MidiEventType.NoteOff;
                    }

                    track.MidiEvents.Add(
                        new MidiEvent { CurrentTime = CurrentTime, Time = time, Type = eventType, Arg1 = channel, Arg2 = data1, Arg3 = data2 });
                }
                else
                {
                    if (status == 0xFF)
                    {
                        // Meta Event
                        var metaEventType = Reader.Read8(data, ref position);

                        // There is a group of meta event types reserved for text events which we store separately
                        if (metaEventType >= 0x01 && metaEventType <= 0x0F)
                        {
                            var textLength = Reader.ReadVarInt(data, ref position);
                            var textValue = Reader.ReadString(data, ref position, textLength);
                            var textEvent = new TextEvent { CurrentTime = CurrentTime, Time = time, Type = metaEventType, Value = textValue };
                            track.TextEvents.Add(textEvent);
                        }
                        else
                        {
                            var data1 = (byte)0;
                            var data2 = (byte)0;

                            // We only handle the few meta events we care about and skip the rest
                            if (ParseMetaEvent(data, ref position, metaEventType, ref data1, ref data2))
                            {
                                track.MidiEvents.Add(
                                    new MidiEvent
                                    {
                                        CurrentTime = CurrentTime,
                                        Time = time,
                                        Type = status,
                                        Arg1 = metaEventType,
                                        Arg2 = data1,
                                        Arg3 = data2
                                    });
                            }
                        }
                    }
                    else if (status == 0xF0 || status == 0xF7)
                    {
                        // SysEx event
                        var length = Reader.ReadVarInt(data, ref position);
                        position += length;
                    }
                    else
                    {
                        ++position;
                    }
                }
            }

            return track;
        }

        private static class Reader
        {
            public static int Read16(byte[] data, ref int i)
            {
                return (data[i++] << 8) | data[i++];
            }

            public static int Read32(byte[] data, ref int i)
            {
                return (data[i++] << 24) | (data[i++] << 16) | (data[i++] << 8) | data[i++];
            }

            public static byte Read8(byte[] data, ref int i)
            {
                return data[i++];
            }

            public static byte[] ReadAllBytesFromStream(Stream input)
            {
                var buffer = new byte[16 * 1024];
                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }

                    return ms.ToArray();
                }
            }

            public static string ReadString(byte[] data, ref int i, int length)
            {
                var result = Encoding.ASCII.GetString(data, i, length);
                i += length;
                return result;
            }

            public static int ReadVarInt(byte[] data, ref int i)
            {
                var result = (int)data[i++];

                if ((result & 0x80) == 0)
                {
                    return result;
                }

                for (var j = 0; j < 3; j++)
                {
                    var value = (int)data[i++];

                    result = (result << 7) | (value & 0x7F);

                    if ((value & 0x80) == 0)
                    {
                        break;
                    }
                }

                return result;
            }
        }
    }

    public class MidiTrack
    {
        public int Index;

        public string TrackName;

        public int Channel;

        public List<MidiEvent> MidiEvents = new List<MidiEvent>();

        public List<TextEvent> TextEvents = new List<TextEvent>();
    }

    public struct MidiEvent
    {
        public int CurrentTime;
        public int Time;

        public byte Type;

        public byte Arg1;

        public byte Arg2;

        public byte Arg3;

        public MidiEventType MidiEventType => (MidiEventType)this.Type;

        public MetaEventType MetaEventType => (MetaEventType)this.Arg1;

        public int Channel => this.Arg1;

        public int Note => this.Arg2;

        public int Velocity => this.Arg3;

        public ControlChangeType ControlChangeType => (ControlChangeType)this.Arg2;

        public int Value => this.Arg3;
    }

    public struct TextEvent
    {
        public int CurrentTime;

        public int Time;

        public byte Type;

        public string Value;

        public TextEventType TextEventType => (TextEventType)this.Type;
    }

    public enum MidiEventType : byte
    {
        NoteOff = 0x80,

        NoteOn = 0x90,

        KeyAfterTouch = 0xA0,

        ControlChange = 0xB0,

        ProgramChange = 0xC0,

        ChannelAfterTouch = 0xD0,

        PitchBendChange = 0xE0,

        MetaEvent = 0xFF
    }

    public enum ControlChangeType : byte
    {
        BankSelect = 0x00,

        Modulation = 0x01,

        Volume = 0x07,

        Balance = 0x08,

        Pan = 0x0A,

        Sustain = 0x40
    }

    public enum TextEventType : byte
    {
        Text = 0x01,

        TrackName = 0x03,

        Lyric = 0x05,
    }

    public enum MetaEventType : byte
    {
        Tempo = 0x51,

        TimeSignature = 0x58,

        KeySignature = 0x59
    }
}

