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
    [Authorize]
    public class TournamentsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Tournaments
        public async Task<ActionResult> Index()
        {
            return View(await db.Tournaments.ToListAsync());
        }

        // GET: Tournaments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.Include(t => t.TournamentGroups).Where(t => t.TournamentId == id).FirstOrDefaultAsync();//.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tournaments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TournamentView view)
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
                view.Logo = pic;
                var tournament = GetTournoment(view);

                db.Tournaments.Add(tournament);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        // GET: Tournaments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            var view = SetTournoment(tournament);
            return View(view);
        }

        // POST: Tournaments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TournamentView view)
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
                view.Logo = pic;

                var tournament = GetTournoment(view);

                db.Entry(tournament).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Tournaments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tournament tournament = await db.Tournaments.FindAsync(id);
            db.Tournaments.Remove(tournament);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #region Tournament Team
        public ActionResult CreateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentGroup = db.TournamentGroups.Find(id);

            var tournamentTeamView = new TournamentTeamView
            {
                TournamentGroupId = tournamentGroup.TournamentGroupId,
            };

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId), "TeamId", "Name");
            return View(tournamentTeamView);
        }

        // POST: Tournaments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {
                var tournamentTeam = GetTournamentTeam(view);
                db.TournamentTeams.Add(tournamentTeam);
                await db.SaveChangesAsync();
                return RedirectToAction($"DetailsGroup/{view.TournamentGroupId}");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId), "TeamId", "Name");
            return View(view);
        }

        TournamentTeam GetTournamentTeam(TournamentTeamView view)
        {
            TournamentTeam _returnView = new TournamentTeam
            {
                AgainstGoals = view.AgainstGoals,
                FavorGoals = view.FavorGoals,
                MatchesLost = view.MatchesLost,
                MatchesPlayed = view.MatchesPlayed,
                MatchesTied = view.MatchesTied,
                MatchesWon = view.MatchesWon,
                Points = view.Points,
                Position = view.Position,
                TeamId = view.TeamId,
                TournamentGroupId = view.TournamentGroupId,
                TournamentTeamId = view.TournamentTeamId,
                Team = view.Team,
                TournamentGroup = view.TournamentGroup
            };
            return _returnView;
        }

        TournamentTeamView GetTournamentTeamView(TournamentTeam Team)
        {
            TournamentTeamView _returnView = new TournamentTeamView
            {
                LeagueId = Team.Team.LeagueId,
                AgainstGoals = Team.AgainstGoals,
                FavorGoals = Team.FavorGoals,
                MatchesLost = Team.MatchesLost,
                MatchesPlayed = Team.MatchesPlayed,
                MatchesTied = Team.MatchesTied,
                MatchesWon = Team.MatchesWon,
                Points = Team.Points,
                Position = Team.Position,
                TeamId = Team.TeamId,
                TournamentGroupId = Team.TournamentGroupId,
                TournamentGroup = Team.TournamentGroup,
                TournamentTeamId = Team.TournamentTeamId,
                Team = Team.Team
            };

            return _returnView;
        }
        public async Task<ActionResult> EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentTeam = await db.TournamentTeams.FindAsync(id);
            var tournamentTeamView = GetTournamentTeamView(tournamentTeam);

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", tournamentTeam.Team.LeagueId);
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.LeagueId == tournamentTeam.Team.LeagueId), "TeamId", "Name", tournamentTeam.Team.TeamId);
            return View(tournamentTeamView);
        }

        // POST: Tournaments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {
                var tournamentTeam = GetTournamentTeam(view);
                db.Entry(tournamentTeam).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction($"DetailsGroup/{view.TournamentGroupId}");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId), "TeamId", "Name");
            return View(view);
        }

        // GET: Tournaments/Delete/5
        public async Task<ActionResult> DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentTeam tournamentTeam = await db.TournamentTeams.FindAsync(id);
            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }

            db.Entry(tournamentTeam).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return RedirectToAction($"DetailsGroup/{tournamentTeam.TournamentGroupId}");
        }

        #endregion
        #region Tournament Group
        public ActionResult CreateGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = db.Tournaments.Find(id);

            var tournamentGroup = new TournamentGroup
            {
                TournamentId = tournament.TournamentId
            };

            return View(tournamentGroup);
        }

        // POST: Tournaments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroup(TournamentGroup view)
        {
            if (ModelState.IsValid)
            {
                db.TournamentGroups.Add(view);
                await db.SaveChangesAsync();
                return RedirectToAction($"Details/{view.TournamentId}");
            }

            return View(view);
        }

        private Tournament GetTournoment(TournamentView view)
        {
            return new Tournament
            {
                TournamentId = view.TournamentId,
                IsActive = view.IsActive,
                Logo = view.Logo,
                Name = view.Name,
                Order = view.Order,
                TournamentGroups = view.TournamentGroups
            };
        }

        public async Task<ActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentGroup tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            return View(tournamentGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditGroup(TournamentGroup view)
        {
            if (ModelState.IsValid)
            {
                db.Entry(view).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction($"Details/{view.TournamentId}");
            }
            return View(view);
        }

        public async Task<ActionResult> DetailsGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentGroup detailsGroup = await db.TournamentGroups.FindAsync(id);

            if (detailsGroup == null)
                return HttpNotFound();

            return View(detailsGroup);
        }

        public async Task<ActionResult> DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentGroup tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            db.TournamentGroups.Remove(tournamentGroup);
            await db.SaveChangesAsync();
            return RedirectToAction($"Details/{tournamentGroup.TournamentId}");

        }

        private object SetTournoment(Tournament tournament)
        {
            return new TournamentView
            {
                TournamentId = tournament.TournamentId,
                IsActive = tournament.IsActive,
                Logo = tournament.Logo,
                Name = tournament.Name,
                Order = tournament.Order,
                TournamentGroups = tournament.TournamentGroups
            };
        }

        #endregion


        #region Modulo de Fecha Tournament Group

        public async Task<ActionResult> CreateDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tournament tournament = await db.Tournaments.FindAsync(id);

            Date date = new Date { TournamentId = tournament.TournamentId };

            return View(date);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDate(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Dates.Add(date);
                await db.SaveChangesAsync();
                return RedirectToAction($"Details/{date.TournamentId}");
            }

            return View(date);
        }

        public async Task<ActionResult> EditDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Date date = await db.Dates.FindAsync(id);

            return View(date);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDate(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Entry(date).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction($"Details/{date.TournamentId}");
            }

            return View(date);
        }

        public async Task<ActionResult> DetailDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var date = await db.Dates.FindAsync(id);

            if (date == null)
                return HttpNotFound();

            return View(date);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDate(int id)
        {
            Date date = await db.Dates.FindAsync(id);
            db.Dates.Remove(date);
            await db.SaveChangesAsync();
            return RedirectToAction($"Details/{date.TournamentId}");
        }
        #endregion

        #region Match
        public async Task<ActionResult> CreateMatch(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var date = await db.Dates.FindAsync(id);

            if (date == null)
                return HttpNotFound();

            var match = new MatchView
            {
                DateId = date.DateId,
                StatusId = 1
            };

            ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.LocalId = new SelectList(db.Teams.Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId), "TeamId", "Name");

            ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.VisitorId = new SelectList(db.Teams.Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId), "TeamId", "Name");

            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId == date.TournamentId).OrderBy(tg => tg.Name), "TournamentGroupId", "Name");

            return View(match);
        }

        // POST: Matches/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMatch(MatchView view)
        {
            if (ModelState.IsValid)
            {

                var match = ToMatch(view);

                db.Matches.Add(match);
                await db.SaveChangesAsync();
                return RedirectToAction($"DetailDate/{match.DateId}");
            }

            ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.LocalId = new SelectList(db.Teams.Where(t => t.LeagueId == view.LocalId), "TeamId", "Name",view.LocalId);

            ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.VisitorId = new SelectList(db.Teams.Where(t => t.LeagueId == view.VisitorId), "TeamId", "Name",view.VisitorId);

            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId == view.Date.TournamentId).OrderBy(tg => tg.Name), "TournamentGroupId", "Name");

            return View(view);
        }

        private Match ToMatch(MatchView view)
        {

            var match = new Match
            {
                DateId = view.DateId,
                DateTime = Convert.ToDateTime($"{view.DateString} {view.TimeString}"),
                LocalId = view.LocalId,
                MatchId = view.MatchId,
                StatusId = view.StatusId,
                TournamentGroupId = view.TournamentGroupId,
                VisitorId = view.VisitorId,
            };
            return match;
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
