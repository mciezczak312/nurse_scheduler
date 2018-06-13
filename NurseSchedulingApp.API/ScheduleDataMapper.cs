using System;
using System.Collections.Generic;
using System.Text;
using NurseSchedulingApp.API;

namespace NurseSchedulingApp
{
    class ScheduleDataMapper
    {
        public Dictionary<int, string> NursesList { get; set; }
        
        public ScheduleDataMapper()
        {
            NursesList = new Dictionary<int, string>
            {
                {0, "Magda"},
                {1, "Pola"},
                {2, "Kamila"},
                {3, "Katarzyna"},
                {4, "Blanka"},
                {5, "Tamara"},
                {6, "Beata"},
                {7, "Bożena"},
                {8, "Nina"},
                {9, "Weronika"},
                {10, "Ania"},
                {11, "Teresa"},
                {12, "Natalia"},
                {13, "Aleksandra"},
                {14, "Adriana"},
                {15, "Joanna"}
            };
        }

        public IEnumerable<IEnumerable<IEnumerable<ScheduleDataDTO>>> MapScheduleToDTO(int [,] solution, int upperBound = 35*5)
        {
            var scheduleData = new List<List<List<ScheduleDataDTO>>>();
            var week = -1;
            for (int i = 0; i < 5; i++)
            {
                scheduleData.Add(new List<List<ScheduleDataDTO>>());
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    scheduleData[i].Add(new List<ScheduleDataDTO>());
                }
                
            }
            

            for (int i = 0; i < upperBound; i++)
            {
                int day = (int)Math.Floor(i / 5.0) % 7;
                int shiftType = i % 5;
                if (i % 35 == 0)
                {
                    week++;
                }
                if (shiftType == 4) continue;
                for (int j = 0; j < 16; j++)
                {
                    if (solution[j, i] == 1)
                    {
                        var obj = new ScheduleDataDTO();
                        obj.NurseId = j;
                        obj.NurseName = NursesList[j];
                        switch (shiftType)
                        {
                            case 0:
                                obj.Shift = "EARLY" ;
                                break;
                            case 1:
                                obj.Shift = "DAY";
                                break;
                            case 2:
                                obj.Shift = "LATE";
                                break;
                            case 3:
                                obj.Shift = "NIGHT";
                                break;
                            default:
                                throw new ArgumentException(nameof(shiftType));
                        }
                        scheduleData[week][day].Add(obj);
                    }
                }

            }

            return scheduleData;
        }
    }
}
