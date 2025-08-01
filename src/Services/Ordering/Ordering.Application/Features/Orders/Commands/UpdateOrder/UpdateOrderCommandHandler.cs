﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ILogger<UpdateOrderCommandHandler> logger)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToUpdate == null)
            {
                _logger.LogError("No order found in database");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            _mapper.Map(request, orderToUpdate,typeof(UpdateOrderCommand),typeof(Order));
            await _orderRepository.UpdateAsync(orderToUpdate);
            _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");
            return Unit.Value;
        }
    }
}
