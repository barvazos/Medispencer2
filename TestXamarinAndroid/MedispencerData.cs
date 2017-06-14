using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TestXamarinAndroid
{
    public class Perscription
    {
        public Perscription(string pillType, string comment, string drName, string[] times,
            int pillsInPack, int cellNum)
        {
            m_pillType = pillType;
            m_comment = comment;
            m_drName = drName;
            m_times = times;
            m_pillsInPack = pillsInPack;
            m_cellNum = cellNum;
        }

        public string m_pillType;
        public string m_comment;
        public string m_drName;
        public string[] m_times;
        public int m_pillsInPack;
        public int m_cellNum;
    }

    public class Cell
    {
        public Cell(int cellNum, string pillType, int numOfPills)
        {
            m_cellNumber = cellNum;
            m_pillType = pillType;
            m_numOfPills = numOfPills;
        }

        public string m_pillType;
        public int m_numOfPills;
        public int m_cellNumber;
    }

    public class MedispencerData
    {
        public static string[] times = new string[] { "08:00:00", "13:00:00" };

        public static Perscription[] s_perscription = new Perscription[]{
            new Perscription("GreanPill", "After food", "Dr d", times, 12, 1),
            new Perscription("BlaPill", "After food", "Dr d", times, 20, 2),
            new Perscription("Akamol", "After food", "Dr d", times, 20, 3)
        };

        public static Cell[] s_cells = new Cell[]{
            new Cell(1, "GreanPill", 12),
            new Cell(2, "BlaPill", 20),
            new Cell(3, "Akamol", 20)
        };
    }
}