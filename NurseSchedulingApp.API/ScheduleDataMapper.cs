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
            NursesList = new Dictionary<int, string>();
            NursesList.Add(0, "Magda");
            NursesList.Add(1, "Pola");
            NursesList.Add(2, "Kamila");
            NursesList.Add(3, "Katarzyna");
            NursesList.Add(4, "Blanka");
            NursesList.Add(5, "Tamara");
            NursesList.Add(6, "Beata");
            NursesList.Add(7, "Bożena");
            NursesList.Add(8, "Nina");
            NursesList.Add(9, "Weronika");
            NursesList.Add(10, "Ania");
            NursesList.Add(11, "Teresa");
            NursesList.Add(12, "Natalia");
            NursesList.Add(13, "Aleksandra");
            NursesList.Add(14, "Adriana");
            NursesList.Add(15, "Joanna");
        }

        public IEnumerable<IEnumerable<IEnumerable<ScheduleDataDTO>>> MapScheduleToDTO(int [,] solution)
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
            

            for (int i = 0; i < 35*5; i++)
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
