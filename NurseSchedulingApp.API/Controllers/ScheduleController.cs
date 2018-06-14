using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace NurseSchedulingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private ScheduleDataMapper _mapper;
        public ScheduleController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _mapper = new ScheduleDataMapper(); //di here
        }

        [Route("uploadFile"), HttpPost, DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                const string folderName = "Upload";
                var webRootPath = _hostingEnvironment.WebRootPath;
                var newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length <= 0) return Json("Upload Successful.");

                const string fileName = "first_week_schedule.txt";
                var fullPath = Path.Combine(newPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Json("Upload Successful.");
            }
            catch (Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var parser = new FirstWeekParser();

            const string folderName = "Upload";
            var webRootPath = _hostingEnvironment.WebRootPath;
            var newPath = Path.Combine(webRootPath, folderName);
            const string fileName = "first_week_schedule.txt";
            var fullPath = Path.Combine(newPath, fileName);
            try
            {
                var solver = new Solver(parser.GetFirstWeekFromFile(fullPath, true));
                var testResults = new string[2];
                while (true)
                {
                    int solverRes;
                    do
                    {
                        solverRes = solver.Solve();
                    } while (solverRes == 0);
                    
                    

                    testResults = solver.RunTests().Split(";;");

                    var tmp = JObject.Parse(testResults[0]);
                    bool valid = true;
                    foreach (var obj in tmp)
                    {
                        if (obj.Key != "5")
                        {
                            if (obj.Value.ToString() != "0") valid = false;
                        }
                    }

                    if (solverRes == 1 && valid) break;
                }


                var dtoSchedule = _mapper.MapScheduleToDTO(solver.Solution);
                var dtoFirstWeek = _mapper.MapScheduleToDTO(solver.FirstWeek, 35);

                return Ok(new SolverResponse
                {
                    FirstWeek = dtoFirstWeek,
                    Schedule = dtoSchedule,
                    HardConstraintsTestsResult = JObject.Parse(testResults[0]),
                    SoftConstraintsTestsResult = JObject.Parse(testResults[1])
                });

            }
            catch (FileNotFoundException e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpGet, Route("nursesList")]
        public IEnumerable<NurseDTO> GetNursesList()
        {
            return _mapper.GetNursesList();
        }
    }
    
}
