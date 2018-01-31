using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio;

namespace Backend.Controllers
{
    public class GenericController : Controller
    {
        private DataContext db = new DataContext();
        // GET: Generic
        public JsonResult GetTournamentTeam(int leagueId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var team = db.Teams.Where(t => t.LeagueId == leagueId).OrderBy(t => t.Name);
            return Json(team);
        }

        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}