using AutoMapper;
using iLG.API.Extensions;
using iLG.API.Models.Responses;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace iLG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HobbyController(IHobbyRepository hobbyRepository, IMapper mapper) : ControllerBase
    {
        private readonly IHobbyRepository _hobbyRepository = hobbyRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetListHobby()
        {
            var hobbies = await _hobbyRepository.GetListAsync();
            var hobbiesResponse = _mapper.Map<List<HobbyResponse>>(hobbies);
            var response = new ApiResponse(hobbiesResponse).GetResult(200, "Sucessful");
            return response;
        }
    }
}