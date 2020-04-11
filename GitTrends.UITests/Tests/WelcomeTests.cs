﻿using System.Linq;
using System.Threading.Tasks;
using GitTrends.Mobile.Shared;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace GitTrends.UITests
{
    [TestFixture(Platform.Android, UserType.LoggedIn)]
    [TestFixture(Platform.iOS, UserType.LoggedIn)]
    [TestFixture(Platform.Android, UserType.Demo)]
    [TestFixture(Platform.iOS, UserType.Demo)]
    class WelcomeTests : BaseTest
    {
        public WelcomeTests(Platform platform, UserType userType) : base(platform, userType)
        {
        }

        public override async Task BeforeEachTest()
        {
            await base.BeforeEachTest();

            RepositoryPage.TapSettingsButton();

            SettingsPage.DismissSyncfusionLicensePopup();
            SettingsPage.WaitForGitHubLoginToComplete();

            //Assert
            Assert.AreEqual(GitHubLoginButtonConstants.Disconnect, SettingsPage.GitHubButtonText);

            //Act
            SettingsPage.TapGitHubButton();
            SettingsPage.WaitForGitHubLogoutToComplete();

            //Assert
            Assert.AreEqual(GitHubLoginButtonConstants.ConnectWithGitHub, SettingsPage.GitHubButtonText);

            SettingsPage.TapBackButton();

            await WelcomePage.WaitForPageToLoad();
        }

        [Test]
        public async Task VerifyDemoButton()
        {
            //Arrange
            Repository demoUserRepository;

            //Act
            WelcomePage.TapTryDemoButton();
            WelcomePage.WaitForActivityIndicator();

            await RepositoryPage.WaitForPageToLoad().ConfigureAwait(false);
            await RepositoryPage.WaitForNoPullToRefreshIndicator().ConfigureAwait(false);

            //Assert
            demoUserRepository = RepositoryPage.VisibleCollection.First();

            Assert.AreEqual(DemoDataConstants.Alias, demoUserRepository.OwnerLogin);
        }

        [Test]
        public async Task VerifyConnectToGitHubButton()
        {
            //Arrange

            //Act
            WelcomePage.TapConnectToGitHubButton();
            WelcomePage.WaitForActivityIndicator();

            //Assert
            await Task.Delay(1000).ConfigureAwait(false);
            if (App is iOSApp)
                Assert.IsTrue(WelcomePage.IsBrowserOpen);
        }
    }
}