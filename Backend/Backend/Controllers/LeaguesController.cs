using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Backend.Models;
using Backend.Helpers;

namespace Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeaguesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Leagues
        public async Task<ActionResult> Index()
        {
            return View(await db.Leagues.ToListAsync());
        }

        // GET: Leagues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // GET: Leagues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leagues/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeagueView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var league = ToLeague(view);
                league.Logo = pic;
                db.Leagues.Add(league);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private League ToLeague(LeagueView league)
        {
            return new League
            {
                LeagueId = league.LeagueId,
                Logo = league.Logo,
                Name = league.Name,
                Teams = league.Teams
            };
        }

        // GET: Leagues/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }

            var view = ToView(league);

            return View(view);
        }

        private LeagueView ToView(League league)
        {
            return new LeagueView
            {
                LeagueId = league.LeagueId,
                Logo = league.Logo,
                Name = league.Name,
                Teams = league.Teams
            };
        }

        // POST: Leagues/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeagueView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Logo;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var league = ToLeague(view);
                league.Logo = pic;

                db.Entry(league).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }



            return View(view);
        }

        // GET: Leagues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            League league = await db.Leagues.FindAsync(id);
            db.Leagues.Remove(league);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        #region Controladores de Team
        public ActionResult CreateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var league = db.Leagues.Find(id);

            var team = new TeamView { LeagueId = league.LeagueId };

            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TeamView teamview)
        {
            var pic = string.Empty;
            var folder = "~/Content/Logos";

            if (teamview.LogoFile != null)
            {
                pic = FilesHelper.UploadPhoto(teamview.LogoFile, folder);
                pic = $"{folder}/{pic}";
            }

            teamview.Logo = pic;

            var team = new Team
            {
                LeagueId = teamview.LeagueId,
                Fans = teamview.Fans,
                Initials = teamview.Initials,
                League = teamview.League,
                Locals = teamview.Locals,
                Logo = teamview.Logo,
                Name = teamview.Name,
                Visitors = teamview.Visitors
            };

            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                await db.SaveChangesAsync();
                return RedirectToAction($"Details/{team.LeagueId}");
            }

            return View(teamview);
        }

        public ActionResult EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var team = db.Teams.Find(id);

            var teamview = new TeamView
            {
                LeagueId = team.LeagueId,
                Logo = team.Logo,
                Initials = team.Initials,
                Name = team.Name,
                TeamId = team.TeamId
            };

            return View(teamview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TeamView teamview)
        {
            var pic = teamview.Logo;
            var folder = "~/Content/Logos";

            if (teamview.LogoFile != null)
            {
                pic = FilesHelper.UploadPhoto(teamview.LogoFile, folder);
                pic = $"{folder}/{pic}";
            }

            teamview.Logo = pic;

            var team = new Team
            {
                LeagueId = teamview.LeagueId,
                Initials = teamview.Initials,
                Logo = teamview.Logo,
                Name = teamview.Name,
                TeamId = teamview.TeamId
            };

            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction($"Details/{team.LeagueId}");
            }

            return View(teamview);
        }

        public async Task<ActionResult> DetailsTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        public async Task<ActionResult> DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }

            db.Entry(team).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return RedirectToAction($"Details/{team.LeagueId}");
        }

        #endregion

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
