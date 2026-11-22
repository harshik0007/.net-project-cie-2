using System;
using System.Collections.Generic;
using CIE_2_PROJECT.Delegates;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Services
{
    public sealed class ContractService : WorkflowServiceBase, IContractService
    {
        private readonly IContractRepository contracts;

        public ContractService(IContractRepository contracts, WorkflowAuditLogger auditLogger)
            : base(auditLogger)
        {
            if (contracts == null)
                throw new ArgumentNullException("contracts");
            this.contracts = contracts;
        }

        public Contract GetForClient(int clientId, int contractId)
        {
            if (clientId <= 0 || contractId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");

            Contract contract = contracts.GetById(contractId);
            if (contract == null || contract.ClientID != clientId)
                throw new InvalidOperationException("Contract not found or access is denied.");
            return contract;
        }

        public Contract GetForClientByProjectId(int clientId, int projectId)
        {
            if (clientId <= 0 || projectId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");
            Contract contract = contracts.GetByProjectId(projectId);
            if (contract == null || contract.ClientID != clientId)
                throw new InvalidOperationException("Contract not found or access is denied.");
            return contract;
        }

        public IList<Contract> GetForClient(int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentOutOfRangeException("clientId");
            return contracts.GetByClientId(clientId);
        }

        public IList<Contract> GetForFreelancerHistory(int freelancerId)
        {
            if (freelancerId <= 0)
                throw new ArgumentOutOfRangeException("freelancerId");
            return contracts.GetByFreelancerId(freelancerId);
        }

        public Contract GetForFreelancer(int freelancerId, int contractId)
        {
            if (freelancerId <= 0 || contractId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");

            Contract contract = contracts.GetById(contractId);
            if (contract == null || contract.FreelancerID != freelancerId)
                throw new InvalidOperationException("Contract not found or access is denied.");
            return contract;
        }

        public IList<Contract> GetActiveForFreelancer(int freelancerId)
        {
            if (freelancerId <= 0)
                throw new ArgumentOutOfRangeException("freelancerId");
            return contracts.GetActiveByFreelancerId(freelancerId);
        }
    }
}
