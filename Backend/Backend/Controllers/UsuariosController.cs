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
    public class UsuariosController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.FavoriteTeam).Include(u => u.UserType);
            return View(await users.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.FavoriteLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name");
            ViewBag.FavoriteTeamId = new SelectList(db.Teams.Where(t => t.TeamId == db.Teams.FirstOrDefault().TeamId).OrderBy(t => t.Name), "TeamId", "Name");
            ViewBag.UserTypeId = new SelectList(db.UserTypes.OrderBy(ut => ut.Name), "UserTypeId", "Name");
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Users";

                if (view.PictureFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.PictureFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                    view.Picture = pic;
                }

                var user = ToUser(view);

                db.Users.Add(user);
                await db.SaveChangesAsync();
                UsersHelper.CreateUserASP(view.Email, "User", view.Password);
                return RedirectToAction("Index");
            }

            ViewBag.FavoriteLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name");
            ViewBag.FavoriteTeamId = new SelectList(db.Teams.OrderBy(t => t.Name), "TeamId", "Name", db.Leagues.FirstOrDefault().LeagueId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes.OrderBy(ut => ut.Name), "UserTypeId", "Name");

            return View(view);
        }

        private User ToUser(UserView view)
        {
            return new User
            {
                Email = view.Email,
                FavoriteTeam = view.FavoriteTeam,
                FavoriteTeamId = view.FavoriteTeamId,
                FirstName = view.FirstName,
                GroupUsers = view.GroupUsers,
                LastName = view.LastName,
                NickName = view.NickName,
                Picture = view.Picture,
                Points = view.Points,
                Predictions = view.Predictions,
                UserGroups = view.UserGroups,
                UserId = view.UserId,
                UserType = view.UserType,
                UserTypeId = view.UserTypeId
            };
        }

        // GET: Usuarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", user.FavoriteTeamId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", user.UserTypeId);
            return View(user);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", user.FavoriteTeamId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", user.UserTypeId);
            return View(user);
        }

        // GET: Usuarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
