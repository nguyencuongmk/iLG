using AutoMapper;
using iLG.API.Extensions;
using iLG.API.Models.Responses;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iLG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HobbyController(IHobbyCategoryRepository hobbyRepository, IMapper mapper) : ControllerBase
    {
        private readonly IHobbyCategoryRepository _hobbyRepository = hobbyRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetListHobby()
        {
            var hobbies = await _hobbyRepository.GetListAsync();
            var hobbiesResponse = _mapper.Map<List<HobbyCategoryResponse>>(hobbies);
            var response = new ApiResponse(hobbiesResponse).GetResult(200, "Sucessful");
            return response;
        }
    }
}