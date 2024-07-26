﻿using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Queries.GetChat;

public class GetChatQueryHandler : IRequestHandler<GetChatQuery, Result<ChatModelResult>>
{
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;

    public GetChatQueryHandler(IChatRepository chatRepository, IMessageRepository messageRepository)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
    }
    
    public async Task<Result<ChatModelResult>> Handle(GetChatQuery request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.GetByIdAsync(request.ChatId, true);

        if (chat is null)
            return Result<ChatModelResult>.Invalid("Chat was not found");

        if (chat.Members.Find(x => x.UserId == request.MemberId) is null)
            return Result<ChatModelResult>.Invalid("User is not a member of this chat");

        chat.Messages = await _messageRepository.GetByChatIdAsync(request.ChatId, request.FromMessage, request.ToMessage);

        return Result<ChatModelResult>.Success(chat.ToResult());
    }
}