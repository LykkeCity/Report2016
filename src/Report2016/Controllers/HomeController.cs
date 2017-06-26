using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report2016.Models;
using Reports2016.Domains;

namespace Report2016.Controllers
{
	
    public class HomeController : Controller
    {

        IVoteTokensRepository _voteTokensRepository;

        public HomeController(IVoteTokensRepository voteTokensRepository)
        {
            _voteTokensRepository = voteTokensRepository;
        }

        public IActionResult Index()
        {
            var userId = this.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Signin");

            return RedirectToAction("Vote");
        }

		[Authorize]
		public IActionResult DoSignIn()
		{
			return RedirectToAction("Index");
		}


		[Authorize]
		public IActionResult Auth()
		{
			return RedirectToAction("Index");
		}

        [HttpGet("/Result")]
		public IActionResult Result()
		{
			return View();
		}

		[HttpGet("/Success")]
		public IActionResult Success()
		{
			return View();
		}

		[HttpGet("/Signin")]
		public IActionResult Signin()
		{
			return View();
		}


		[HttpGet("/Vote")]
        public IActionResult Vote()
		{
			return View();
		}


		[HttpPost("/MyVote")]
		public IActionResult MyVote(MyVoteModel model)
		{

            if (model.NotVoted())
				return Redirect("Vote");
            
            return Content(model.Comment);

		}


        [HttpGet("/Vote/{id}")]
		public async Task<IActionResult> Vote([FromRoute]string id)
		{

            if (string.IsNullOrEmpty(id))
                return Redirect("Signin");

            var token = await _voteTokensRepository.FindTokenAsync(id);

            if (token == null)
                return Redirect("Signin");
            
			return Content(id);

		}

    }
	
}
