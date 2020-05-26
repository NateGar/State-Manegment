using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using StateManegmentLab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace StateManegmentLab.Controllers
{
    public class MovieController : Controller
    {
        public List<RentMovie> savedMovies = new List<RentMovie>();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddMovie(RentMovie movie)
        {
            string movieString = JsonSerializer.Serialize(movie);

            HttpContext.Session.SetString("RentMovieSession", movieString);

            return View();
        }

        public IActionResult AddMovieToList()
        {
            //getting the new item from the Session
            RentMovie savedMovie = JsonSerializer.Deserialize<RentMovie>(HttpContext.Session.GetString("RentMovieSession"));
            //filling the saved list from our List session
            string movieListJSON = HttpContext.Session.GetString("MovieList") ?? "NOPE";
            if (movieListJSON != "NOPE")
            {
                savedMovies = JsonSerializer.Deserialize<List<RentMovie>>(movieListJSON);
            }
            //Adding the session  to the Saved List
            savedMovies.Add(savedMovie);
            //resave the List with the new item in it.
            string movieListJson = JsonSerializer.Serialize(savedMovies);
            HttpContext.Session.SetString("MovieList", movieListJson);

            return RedirectToAction("ListMovies");

        }

        public IActionResult ListMovies()
        {
            savedMovies = JsonSerializer.Deserialize<List<RentMovie>>(HttpContext.Session.GetString("MovieList"));

            return View(savedMovies);
        }
    }
}
