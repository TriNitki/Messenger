﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSG.Messenger.Contracts;
using MSG.Messenger.DataAccess.Entities;
using MSG.Messenger.UseCases.Commands.AddMember;
using MSG.Messenger.UseCases.Commands.CreateGroupChat;
using MSG.Messenger.UseCases.Commands.EditAdmin;
using MSG.Messenger.UseCases.Commands.GetOrCreateDirectChat;
using MSG.Messenger.UseCases.Commands.KickMember;
using MSG.Messenger.UseCases.Commands.LeaveGroupChat;
using MSG.Messenger.UseCases.Commands.RenameChat;
using MSG.Messenger.UseCases.Queries.GetChat;
using MSG.Messenger.UseCases.Queries.GetChats;
using MSG.Security.Authorization;
using Packages.Application.UseCases;

namespace MSG.Messenger.Service.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;

        public ChatController(IMediator mediator, IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        [HttpPost("createGroup")]
        public async Task<IActionResult> CreateGroup(CreateGroupChatRequest request)
        {
            request.Members.Add(_userAccessor.Id);
            var result = await _mediator.Send(new CreateGroupChatCommand(request.Name, request.Members, _userAccessor.Id));
            return result.ToActionResult();
        }

        [HttpPost("createDirect")]
        public async Task<IActionResult> CreateDirect(CreateDirectChatRequest request)
        {
            var result = await _mediator.Send(new GetOrCreateDirectChatCommand(_userAccessor.Id, request.ReceiverId));
            return result.ToActionResult();
        }

        [HttpDelete("{chatId:guid}/leaveGroup")]
        public async Task<IActionResult> LeaveGroup(Guid chatId)
        {
            var result = await _mediator.Send(new LeaveGroupChatCommand(_userAccessor.Id, chatId));
            return result.ToActionResult();
        }

        [HttpDelete("{chatId:guid}/members/{memberId:guid}")]
        public async Task<IActionResult> KickMember(Guid chatId, Guid memberId)
        {
            var result = await _mediator.Send(new KickMemberCommand(chatId, _userAccessor.Id, memberId));
            return result.ToActionResult();
        }

        [HttpPost("{chatId:guid}/members")]
        public async Task<IActionResult> AddMember(Guid chatId, AddMemberRequest request)
        {
            var result = await _mediator.Send(new AddMemberCommand(chatId, _userAccessor.Id, request.MemberId));
            return result.ToActionResult();
        }

        [HttpPatch("{chatId:guid}")]
        public async Task<IActionResult> Rename(Guid chatId, RenameChatRequest request)
        {
            var result = await _mediator.Send(new RenameChatCommand(chatId, _userAccessor.Id, request.Name));
            return result.ToActionResult();
        }

        [HttpPatch("{chatId:guid}/editAdmin/{memberId:guid}")]
        public async Task<IActionResult> EditAdmin(Guid chatId, Guid memberId, EditAdminRequest request)
        {
            var result = await _mediator.Send(new EditAdminCommand(chatId, _userAccessor.Id, memberId, request.IsAdmin));
            return result.ToActionResult();
        }

        [HttpGet("{chatId:guid}")]
        public async Task<IActionResult> GetChat(Guid chatId, [FromQuery] uint fromMessage = 0, [FromQuery] uint toMessage = 25)
        {
            var result = await _mediator.Send(new GetChatQuery(chatId, _userAccessor.Id, (int)fromMessage, (int)toMessage));
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetChats([FromQuery] uint fromChat = 0, [FromQuery] uint toChat = 25)
        {
            var result = await _mediator.Send(new GetChatsQuery(_userAccessor.Id, (int)fromChat, (int)toChat));
            return result.ToActionResult();
        }
    }
}
