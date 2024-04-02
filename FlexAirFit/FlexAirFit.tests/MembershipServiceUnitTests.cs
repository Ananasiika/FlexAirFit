using FlexAirFit.Core.Models;
using FlexAirFit.Application.IRepositories;
using FlexAirFit.Application.IServices;
using FlexAirFit.Application.Exceptions.ServiceException;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlexAirFit.Application.Services;
using Xunit;

namespace FlexAirFit.Tests
{
    public class MembershipServiceUnitTests
    {
        private readonly Mock<IMembershipRepository> _mockMembershipRepository;
        private readonly IMembershipService _membershipService;

        public MembershipServiceUnitTests()
        {
            _mockMembershipRepository = new Mock<IMembershipRepository>();
            _membershipService = new MembershipService(_mockMembershipRepository.Object);
        }

        [Fact]
        public async Task CreateMembership_ShouldCallAddMembershipAsync_WhenMembershipDoesNotExist()
        {
            var membership = new Membership(Guid.NewGuid(), "Gold Membership", TimeSpan.FromDays(30), 100, 7);

            _mockMembershipRepository.Setup(r => r.GetMembershipByIdAsync(membership.Id)).ReturnsAsync((Membership)null);

            await _membershipService.CreateMembership(membership);

            _mockMembershipRepository.Verify(r => r.AddMembershipAsync(membership), Times.Once);
        }

        [Fact]
        public async Task CreateMembership_ShouldThrowMembershipExistsException_WhenMembershipExists()
        {
            var membership = new Membership(Guid.NewGuid(), "Gold Membership", TimeSpan.FromDays(30), 100, 7);

            _mockMembershipRepository.Setup(r => r.GetMembershipByIdAsync(membership.Id)).ReturnsAsync(membership);

            await Assert.ThrowsAsync<MembershipExistsException>(() => _membershipService.CreateMembership(membership));
        }

        [Fact]
        public async Task UpdateMembership_ShouldCallUpdateMembershipAsync_WhenMembershipExists()
        {
            var membership = new Membership(Guid.NewGuid(), "Gold Membership", TimeSpan.FromDays(30), 100, 7);

            _mockMembershipRepository.Setup(r => r.GetMembershipByIdAsync(membership.Id)).ReturnsAsync(membership);

            await _membershipService.UpdateMembership(membership);

            _mockMembershipRepository.Verify(r => r.UpdateMembershipAsync(membership), Times.Once);
        }

        [Fact]
        public async Task UpdateMembership_ShouldThrowMembershipNotFoundException_WhenMembershipDoesNotExist()
        {
            var membership = new Membership(Guid.NewGuid(), "Gold Membership", TimeSpan.FromDays(30), 100, 7);

            _mockMembershipRepository.Setup(r => r.GetMembershipByIdAsync(membership.Id)).ReturnsAsync((Membership)null);

            await Assert.ThrowsAsync<MembershipNotFoundException>(() => _membershipService.UpdateMembership(membership));
        }

        [Fact]
        public async Task DeleteMembership_ShouldCallDeleteMembershipAsync_WhenMembershipExists()
        {
            var membershipId = Guid.NewGuid();

            _mockMembershipRepository.Setup(r => r.GetMembershipByIdAsync(membershipId)).ReturnsAsync(new Membership(membershipId, "Gold Membership", TimeSpan.FromDays(30), 100, 7));

            await _membershipService.DeleteMembership(membershipId);

            _mockMembershipRepository.Verify(r => r.DeleteMembershipAsync(membershipId), Times.Once);
        }

        [Fact]
        public async Task DeleteMembership_ShouldThrowMembershipNotFoundException_WhenMembershipDoesNotExist()
        {
            var membershipId = Guid.NewGuid();

            _mockMembershipRepository.Setup(r => r.GetMembershipByIdAsync(membershipId)).ReturnsAsync((Membership)null);

            await Assert.ThrowsAsync<MembershipNotFoundException>(() => _membershipService.DeleteMembership(membershipId));
        }

        [Fact]
        public async Task GetMembershipById_ShouldReturnMembership_WhenMembershipExists()
        {
            var membershipId = Guid.NewGuid();
            var membership = new Membership(membershipId, "Gold Membership", TimeSpan.FromDays(30), 100, 7);

            _mockMembershipRepository.Setup(r => r.GetMembershipByIdAsync(membershipId)).ReturnsAsync(membership);

            var result = await _membershipService.GetMembershipById(membershipId);

            Assert.Equal(membership, result);
        }

        [Fact]
        public async Task GetMembershipById_ShouldThrowMembershipNotFoundException_WhenMembershipDoesNotExist()
        {
            var membershipId = Guid.NewGuid();

            _mockMembershipRepository.Setup(r => r.GetMembershipByIdAsync(membershipId)).ReturnsAsync((Membership)null);

            await Assert.ThrowsAsync<MembershipNotFoundException>(() => _membershipService.GetMembershipById(membershipId));
        }

        [Fact]
        public async Task GetMemberships_ShouldReturnListOfMemberships_WithLimitAndOffset()
        {
            var memberships = new List<Membership>
            {
                new Membership(Guid.NewGuid(), "Gold Membership", TimeSpan.FromDays(30), 100, 7),
                new Membership(Guid.NewGuid(), "Silver Membership", TimeSpan.FromDays(30), 80, 5),
                new Membership(Guid.NewGuid(), "Bronze Membership", TimeSpan.FromDays(30), 60, 3)
            };

            _mockMembershipRepository.Setup(r => r.GetMembershipsAsync(2, 1)).ReturnsAsync(memberships);

            var result = await _membershipService.GetMemberships(2, 1);

            Assert.Equal(memberships, result);
        }
    }
}
