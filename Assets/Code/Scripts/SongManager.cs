using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    [CreateAssetMenu(menuName = "Scriptable Object/Song Manager")]
    public class SongManager : ScriptableObject
    {

        public SongData currentSongData;

        //the current position of the song (in beats)
        public float songPosInBeats;

        //combo of the song
        public int songCombo;

        //score multiplier
        public int scoreMultiplier = 1;

        //player score
        public int player1Score;
        public int player2Score;

        //hold the player hit notes index 0 = offbeat 1 = perfect 2 = good 3 = miss
        public int[] play1HitNotes = new int[4];
        public int[] play2HitNotes = new int[4];

        //Song HP
        public int HP;



    }
}
