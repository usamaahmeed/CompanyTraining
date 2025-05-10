//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace CompanyTraining.Areas.Company.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize("Company")]
//    public class QuestionController : ControllerBase
//    {
//        private readonly IQuestionRepository _questionRepository;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public QuestionController(IQuestionRepository questionRepository,UserManager<ApplicationUser> userManager) 
//        {
//            this._questionRepository = questionRepository;
//            this._userManager = userManager;
//        }
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] QuestionRequest questionRequest)
//        {
//            var companyApp = await _userManager.GetUserAsync(User);
//            if (companyApp == null)
//                return NotFound();

//        }
//    }
//}
