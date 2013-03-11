using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WMPLib;

namespace moodPlayer
{
    class PlayListCreator
    {
        private List<string> m_filePaths;
        private List<float> m_intensities;
        private List<string> m_playList;

        public PlayListCreator(string fileName)
        {
            m_filePaths = new List<string>();
            m_intensities = new List<float>();
            m_playList = new List<string>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    if (values.Length != 2)
                    {
                        throw new FormatException();
                    }
                    m_filePaths.Add(values[0]);
                    float intensity = float.NegativeInfinity;
                    float.TryParse(values[1], out intensity);
                    if (intensity == float.NegativeInfinity)
                    {
                        throw new FormatException();
                    }
                    m_intensities.Add(intensity);
                }
            }
        }

        private void playSongs()
        {
            WindowsMediaPlayer player = new WindowsMediaPlayer();
            IWMPPlaylist playlist = player.playlistCollection.newPlaylist("myplaylist");
            IWMPMedia media;
            foreach (string file in m_playList)
            {
                media = player.newMedia(file);
                playlist.appendItem(media);
            }
            player.currentPlaylist = playlist;
            player.controls.play();
        }

        public void createPlayList(List<float> intensityValues)
        {
            Random r = new Random();
            foreach (float desiredIntensity in intensityValues)
            {
                int startingPoint = r.Next(0, m_intensities.Count);
                int currentPoint = startingPoint;
                do
                {
                    if (desiredIntensity < 0.5 && m_intensities[currentPoint] == -1)
                    {
                        m_playList.Add(m_filePaths[currentPoint]);
                        break;
                    }
                    if (desiredIntensity >= 0.5 && m_intensities[currentPoint] == 1)
                    {
                        m_playList.Add(m_filePaths[currentPoint]);
                        break;
                    }
                    currentPoint = (currentPoint + 1) % m_intensities.Count;
                }while (currentPoint != startingPoint);
            }

            playSongs();
        }
    }
}
