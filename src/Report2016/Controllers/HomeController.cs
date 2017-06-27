﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report2016.Models;
using Reports2016.Domains;

namespace Report2016.Controllers
{
	
    public class HomeController : Controller
    {

        IVoteTokensRepository _voteTokensRepository;
        IVotesRepository _votesRepository;

        public HomeController(IVoteTokensRepository voteTokensRepository, IVotesRepository votesRepository)
        {
            _voteTokensRepository = voteTokensRepository;
            _votesRepository = votesRepository;
        }

        public IActionResult Index()
        {
            var usereEmail = this.GetUserEmail();

            if (string.IsNullOrEmpty(usereEmail))
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
        [Authorize]
        public async Task<IActionResult> Vote()
		{
            var email = this.GetUserEmail();
			var vote = await _votesRepository.GetAsync(email);

            if (vote != null)
				return RedirectToAction("Success");            

            var viewModel = new VoteViewModel();
			return View(viewModel);
		}


		[HttpPost("/MyVote")]
		public async Task<IActionResult> MyVote(MyVoteContract model)
		{

            if (model.NotVoted())
				return RedirectToAction("Vote");
            
            if (string.IsNullOrEmpty(model.Token))
            {
                var user = this.GetUser();

                if (user == null)
                    return RedirectToAction("Signin");

                model.Email = user.Email;
                model.UserId = user.UserId;

            }
            else
            {
                var token = await _voteTokensRepository.FindTokenAsync(model.Token);

                if (token == null)
					return RedirectToAction("Signin");

                model.Email = token.Email;
            }

            IVote vote = await _votesRepository.GetAsync(model.Email);

            if (vote != null)
                return RedirectToAction("Success");



            await _votesRepository.VoteAsync(model);

            return RedirectToAction("Success");

		}


        [HttpGet("/Vote/{id}")]
		public async Task<IActionResult> Vote([FromRoute]string id)
		{

            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Vote");

            var token = await _voteTokensRepository.FindTokenAsync(id);

            if (token == null)
                return RedirectToAction("Vote");

            var viewModel = new VoteViewModel{
                Token = id
            };

			return View("Vote", viewModel);

		}

    }
	
}
