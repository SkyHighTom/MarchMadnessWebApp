using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MarchMadnessWebApp.Models;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace MarchMadnessWebApp.Controllers;

public class HomeController : Controller
{
    private static string user = "";
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CheckLogin(string name)
    {
        //Track successful login
        bool loginSuccessful = false;

        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            //Check if there is a patron with the given name and cardnum
            var query = from p in db.Users
                        where
                        p.Username.Equals(name)
                        select p;
            //If there any errors checking the count of the query login unsuccessful
            try
            {
                //If there is a line returned by the query that means there is someone in the database
                if (query.Count() == 1)
                {
                    loginSuccessful = true;
                }
            }
            catch
            {
                loginSuccessful = false;
            }
        }
        //Return success false or true based on loginSuccessful
        if (!loginSuccessful)
        {
            return Json(new { success = false });
        }
        else
        {
            user = name;
            return Json(new { success = true });
        }
    }

    [HttpPost]
    public IActionResult RegisterUser(string name)
    {
        //Track successful registration
        bool registerSuccessful = false;

        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            //Check if there is a patron with the given name and cardnum
            var query = from p in db.Users
                        where
                        p.Username.Equals(name)
                        select p;
            //If there any errors checking the count of the query login unsuccessful
            try
            {
                //If there is a line returned by the query that means there is someone in the database
                if (query.Count() == 1)
                {
                    registerSuccessful = false;
                }
                else
                {
                    User user = new User();
                    user.Username = name;
                    user.Score = 0;
                    db.Users.Add(user);
                    var result = db.SaveChanges();
                    if (result == 0) // if there are no changes to the database, there will be no columns affected
                    {
                        return Json(new { success = false });
                    }
                    var teamQuery = from t in db.Teams
                                    select t.TeamName;
                    foreach(var team in teamQuery)
                    {
                        Streak streak = new Streak();
                        streak.Username = name;
                        streak.TeamName = team;
                        streak.Streak1 = 0;
                        db.Streaks.Add(streak);
                    }
                    db.SaveChanges();
                    registerSuccessful = true;
                }
            }
            catch
            {
                registerSuccessful = false;
            }
        }
        //Return success false or true based on loginSuccessful
        if (!registerSuccessful)
        {
            return Json(new { success = false });
        }
        else
        {
            user = name;
            return Json(new { success = true });
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public ActionResult LogOut()
    {
        user = "";
        return Json(new { success = true });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public ActionResult AllMatches()
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            var matchFilter = from m in db.Matches
                              join t1 in db.Teams on m.Team1 equals t1.TeamName into team1Group
                              from t1 in team1Group.DefaultIfEmpty()
                              join t2 in db.Teams on m.Team2 equals t2.TeamName into team2Group
                              from t2 in team2Group.DefaultIfEmpty()
                              select new
                              {
                                  m.Team1,
                                  m.Team2,
                                  Seed1 = t1 != null ? t1.Seed : (int?)null,
                                  Seed2 = t2 != null ? t2.Seed : (int?)null,
                                  m.Round,
                                  m.MatchId
                              };

            var predictionFilter = from p in db.Predictions
                                   where p.Username.Equals(user)
                                   select p;

            var query = from m in matchFilter
                        join p in predictionFilter on m.MatchId equals p.MatchId
                        into predictionsGroup
                        from p in predictionsGroup.DefaultIfEmpty()
                        select new
                        {
                            m.Team1,
                            m.Team2,
                            m.Seed1,
                            m.Seed2,
                            m.Round,
                            Prediction = p != null ? p.Winner : ""
                        } into allMatches
                        orderby allMatches.Round descending, allMatches.Seed1
                        select allMatches;


            var result = query.ToList(); // Execute the query

            //Return the json form of the result of the query as an array
            return Json(result);
        }
    }

    [HttpPost]
    public ActionResult AllMatchesResults()
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            var matchFilter = from m in db.Matches
                              join t1 in db.Teams on m.Team1 equals t1.TeamName into team1Group
                              from t1 in team1Group.DefaultIfEmpty()
                              join t2 in db.Teams on m.Team2 equals t2.TeamName into team2Group
                              from t2 in team2Group.DefaultIfEmpty()
                              select new
                              {
                                  m.Team1,
                                  m.Team2,
                                  Seed1 = t1 != null ? t1.Seed : (int?)null,
                                  Seed2 = t2 != null ? t2.Seed : (int?)null,
                                  m.Round,
                                  m.MatchId
                              };

            var query = from m in matchFilter
                        join r in db.Results on m.MatchId equals r.MatchId
                        into resultsGroup
                        from r in resultsGroup.DefaultIfEmpty()
                        select new
                        {
                            m.Team1,
                            m.Team2,
                            m.Seed1,
                            m.Seed2,
                            m.Round,
                            Result = r != null ? r.Winner : ""
                        } into allMatches
                        orderby allMatches.Round descending, allMatches.Seed1
                        select allMatches;


            var result = query.ToList(); // Execute the query

            //Return the json form of the result of the query as an array
            return Json(result);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public ActionResult AllResults()
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            var query = from m in db.Matches
                        join r in db.Results on m.MatchId equals r.MatchId
                        join t1 in db.Teams on m.Team1 equals t1.TeamName into team1Group
                        from t1 in team1Group.DefaultIfEmpty()
                        join t2 in db.Teams on m.Team2 equals t2.TeamName into team2Group
                        from t2 in team2Group.DefaultIfEmpty()
                        select new
                        {
                            m.Team1,
                            Seed1 = t1 != null ? t1.Seed : (int?)null, // Get Seed1
                            m.Team2,
                            Seed2 = t2 != null ? t2.Seed : (int?)null, // Get Seed2
                            m.Round,
                            r.Winner,
                            r.Team1Score,
                            r.Team2Score
                        } into AllResults
                        orderby AllResults.Round descending, AllResults.Seed1
                        select AllResults;

            var result = query.ToList(); // Execute the query

            //Return the json form of the result of the query as an array
            return Json(result);
        }
    }

    public class ResultsRequest
    {
        public List<Resu> Predictions { get; set; } = new List<Resu>();
    }

    public class Resu
    {
        public string Team1 { get; set; } = "";
        public string Team2 { get; set; } = "";
        public string Winner { get; set; } = "";
        public int Round { get; set; }
    }

    [HttpPost]
    public ActionResult SubmitResults([FromBody] ResultsRequest results)
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            foreach (var result in results.Predictions)
            {
                var round = result.Round;
                var team1 = result.Team1;
                var team2 = result.Team2;
                var winner = result.Winner;
                var query = from m in db.Matches
                            where m.Team2.Equals(team2) && m.Team1.Equals(team1) && m.Round == round
                            select m.MatchId;
                // Query to find existing prediction by MatchId and Username
                var existingResult = db.Results
                    .FirstOrDefault(p => p.MatchId == query.ToArray()[0]);

                if (existingResult != null)
                {
                    // Update existing prediction
                    existingResult.Winner = winner;
                    db.Results.Update(existingResult);
                }
                else
                {
                    Result res = new Result();
                    res.Winner = winner;
                    res.Team1Score = 0;
                    res.Team2Score = 0;
                    res.MatchId = query.ToArray()[0];
                    db.Results.Add(res);
                }
                try
                {   // this code can throw an error so it is wrapped in a try catch loop
                    // the save changes function makes the update pushed to the database
                    var queryResult = db.SaveChanges();

                    if (queryResult == 0) // if there are no changes to the database, there will be no columns affected
                    {
                        return Json(new { success = false });
                    }
                }
                catch
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = true });
        }
    }

    public class PredictionRequest
    {
        public List<Pred> Predictions { get; set; } = new List<Pred>();
    }

    public class Pred
    {
        public string Team1 { get; set; } = "";
        public string Team2 { get; set; } = "";
        public string Winner { get; set; } = "";
        public int Round { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public ActionResult MakePrediction([FromBody] PredictionRequest predictions)
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            foreach(var prediction in predictions.Predictions)
            {
                var round = prediction.Round;
                var team1 = prediction.Team1;
                var team2 = prediction.Team2;
                var winner = prediction.Winner;
                var query = from m in db.Matches
                            where m.Team2.Equals(team2) && m.Team1.Equals(team1) && m.Round == round
                            select m.MatchId;
                // Query to find existing prediction by MatchId and Username
                var existingPrediction = db.Predictions
                    .FirstOrDefault(p => p.MatchId == query.ToArray()[0] && p.Username == user);

                if (existingPrediction != null)
                {
                    // Update existing prediction
                    existingPrediction.Winner = winner;
                    db.Predictions.Update(existingPrediction);
                }
                else
                {
                    Prediction pred = new Prediction();
                    pred.Username = user;
                    pred.Winner = winner;

                    pred.MatchId = query.ToArray()[0];
                    db.Predictions.Add(pred);
                }
                try
                {   // this code can throw an error so it is wrapped in a try catch loop
                    // the save changes function makes the update pushed to the database
                    var result = db.SaveChanges();

                    if (result == 0) // if there are no changes to the database, there will be no columns affected
                    {
                        return Json(new { success = false });
                    }
                }
                catch
                {
                    return Json(new { success = false });
                }
            }
            UpdateScores();
            return Json(new { success = true });
        }
    }

    [HttpPost]
    public void UpdateScores()
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            int score = 0;
            score += CorrectPredictions(64) * 2 + CorrectPredictions(32) * 2 +
                CorrectPredictions(16) * 3;
            // Handle Streaks
            score += HandleStreaks();
            score += CorrectPredictions(8) * 3 + CorrectPredictions(4) * 4 + CorrectPredictions(2) * 4;

            User updateScore = new User();
            updateScore.Score = score;
            updateScore.Username = user;
            db.Users.Update(updateScore);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                
            }
        }
    }

    [HttpPost]
    public int CorrectPredictions(int round)
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            var matches = from m in db.Matches
                          where m.Round == round
                          select m.MatchId;
            var results = from r in db.Results
                          where matches.Contains(r.MatchId)
                          select new
                          {
                              r.Winner,
                              r.MatchId
                          };
            var predictions = from p in db.Predictions
                              where matches.Contains(p.MatchId) && p.Username.Equals(user)
                              select new
                              {
                                  p.Winner,
                                  p.MatchId
                              };
            var matchingPairsCount = (from r in results
                                      join p in predictions on new { r.MatchId, r.Winner } equals new { p.MatchId, p.Winner }
                                      select r).Count();
            return matchingPairsCount;
        }
    }
    [HttpPost]
    public int HandleStreaks()
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            var streaks = from s in db.Streaks
                          where s.Username.Equals(user)
                          select s;
            foreach(var streak in streaks)
            {
                streak.Streak1 = 0;
            }
            db.SaveChanges();
            var matches = from m in db.Matches
                          where m.Round == 64 || m.Round == 32 || m.Round == 16
                          select m.MatchId;
            var predictions = from p in db.Predictions
                              where matches.Contains(p.MatchId) && p.Username.Equals(user)
                              select new
                              {
                                  p.MatchId,
                                  p.Winner
                              };
            var results = from r in db.Results
                          where matches.Contains(r.MatchId)
                          select new
                          {
                              r.MatchId,
                              r.Winner
                          };
            var correctPredictions = (from r in results
                                     join p in predictions on new { r.MatchId, r.Winner } equals new { p.MatchId, p.Winner }
                                     select r.Winner).ToList();
            int streakCount = 0;
            foreach(var prediction in correctPredictions)
            {
                var streak = db.Streaks.Find(user, prediction); // Use primary keys

                if (streak != null)
                {
                    streak.Streak1 += 1; // Update the streak
                    db.SaveChanges(); // Save changes immediately
                    if(streak.Streak1 == 3)
                    {
                        streakCount++;
                    }
                }
            }
            return streakCount;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public ActionResult AllUsers()
    {
        using (MarchMadnessContext db = new MarchMadnessContext())
        {
            var query = from u in db.Users
                        select new
                        {
                            u.Username,
                            u.Score
                        } into allUsers
                        orderby allUsers.Score descending
                        select allUsers;
            //Return the json form of the result of the query as an array
            return Json(query.ToArray());
        }
    }
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (user == "")
            return View("Login");

        return View("Matches");
    }
    public IActionResult Matches()
    {
        return View();
    }
    public IActionResult Results()
    {
        return View();
    }
    public IActionResult Leaderboard()
    {
        return View();
    }
    /// <summary>
    /// Return the Login page.
    /// </summary>
    /// <returns></returns>
    public IActionResult Login()
    {
        user = "";

        ViewData["Message"] = "Please login.";

        return View();
    }

    public IActionResult Register()
    {
        user = "";
        ViewData["Message"] = "Please select username.";
        return View();
    }


    public IActionResult Bracket()
    {
        return View();
    }
    
    public IActionResult SubmitResults()
    {
        return View();
    }

    [HttpPost]
    public JsonResult UpdateButton(string text)
    {
        // You can process the text here if needed
        return Json(new { newText = text });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
