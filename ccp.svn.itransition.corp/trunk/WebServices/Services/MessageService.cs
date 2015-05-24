using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCP.DAL;
using CCP.DAL.DataModels;
using CCP.DAL.Interfaces;

namespace CCP.WebApi.Services
{
    public class MessageService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IRepository<Message> _messageRepository;

        public MessageService()
        {
            _messageRepository = _unitOfWork.MessageRepository;
        }

        public void SaveMessageForInitiator(long actionAuthorId, long recipientId, long previousStatusId, long currentStatusId)
        {
            var message = new Message()
            {
                ActionAuthorId = actionAuthorId,
                RecipientId = recipientId,
                PreviousContractStatusId = previousStatusId,
                CurrentContractStatusId = currentStatusId,
                Text = "Your contract status was changed from \"{0}\" to \"{1}\""
            };
            _messageRepository.Insert(message);
            _unitOfWork.Save();
        }

        public void SaveMessageForInitiator(long recipientId, long previousStatusId, long currentStatusId)
        {
            var message = new Message()
            {
                RecipientId = recipientId,
                PreviousContractStatusId = previousStatusId,
                CurrentContractStatusId = currentStatusId,
                Text = "Your contract status was changed from \"{0}\" to \"{1}\""
            };
            _messageRepository.Insert(message);
            _unitOfWork.Save();
        }

        public void SaveMessageForApprover(long salesPersonId, long recipientId)
        {
            var message = new Message()
            {
                RecipientId = recipientId,
                SalesPersonId = salesPersonId,
                Text = "Contract was assigned to you."
            };
            _messageRepository.Insert(message);
            _unitOfWork.Save();
        }
    }
}