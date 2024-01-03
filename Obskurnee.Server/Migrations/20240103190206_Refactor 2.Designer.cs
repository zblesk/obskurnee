﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Obskurnee.Services;

#nullable disable

namespace Obskurnee.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240103190206_Refactor 2")]
    partial class Refactor2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.25");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Obskurnee.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BookDiscussionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BookPollId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RoundId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BookId");

                    b.HasIndex("PostId");

                    b.HasIndex("RoundId")
                        .IsUnique();

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Obskurnee.Models.BookclubReview", b =>
                {
                    b.Property<string>("ReviewId")
                        .HasColumnType("TEXT");

                    b.Property<int>("BookId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("GoodreadsBookId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReviewText")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReviewUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("ReviewId");

                    b.HasIndex("BookId");

                    b.ToTable("BookclubReviews");
                });

            modelBuilder.Entity("Obskurnee.Models.Bookworm", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AboutMe")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ExternalProfileSystem")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalProfileUrl")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("GoodreadsProfileUrl");

                    b.Property<bool>("IsBot")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Language")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LoginEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Obskurnee.Models.Discussion", b =>
                {
                    b.Property<int>("DiscussionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PollId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoundId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Topic")
                        .HasColumnType("INTEGER");

                    b.HasKey("DiscussionId");

                    b.HasIndex("RoundId");

                    b.ToTable("Discussions");
                });

            modelBuilder.Entity("Obskurnee.Models.ExternalReview", b =>
                {
                    b.Property<string>("ReviewId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Author")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalBookId")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("GoodreadsBookId");

                    b.Property<int>("ExternalSystem")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<int>("Kind")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReviewText")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReviewUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Series")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ReviewId");

                    b.ToTable("GoodreadsReviews");
                });

            modelBuilder.Entity("Obskurnee.Models.GoodreadsBookInfo", b =>
                {
                    b.Property<int>("GoodreadsBookInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PageCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("GoodreadsBookInfoId");

                    b.ToTable("GoodreadsBookInfos");
                });

            modelBuilder.Entity("Obskurnee.Models.NewsletterSubscription", b =>
                {
                    b.Property<string>("NewsletterSubscriptionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("NewsletterName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("NewsletterSubscriptionId");

                    b.HasIndex("NewsletterName");

                    b.HasIndex("UserId");

                    b.ToTable("NewsletterSubscriptions");
                });

            modelBuilder.Entity("Obskurnee.Models.Poll", b =>
                {
                    b.Property<int>("PollId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DiscussionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FollowupLinkSerialized")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTiebreaker")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PreviousPollId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResultsSerialized")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("RoundId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Topic")
                        .HasColumnType("INTEGER");

                    b.HasKey("PollId");

                    b.HasIndex("RoundId");

                    b.ToTable("Polls");
                });

            modelBuilder.Entity("Obskurnee.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("DiscussionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PageCount")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ParentPostId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ParentRecommendationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("PostId");

                    b.HasIndex("DiscussionId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Obskurnee.Models.Recommendation", b =>
                {
                    b.Property<int>("RecommendationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PageCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RecommendationId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("Obskurnee.Models.Round", b =>
                {
                    b.Property<int>("RoundId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BookDiscussionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BookId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BookPollId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BookTiebreakerPollId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("Kind")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ThemeDiscussionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ThemePollId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ThemeTiebreakerPollId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RoundId");

                    b.HasIndex("BookDiscussionId");

                    b.HasIndex("BookPollId");

                    b.HasIndex("BookTiebreakerPollId");

                    b.HasIndex("ThemeDiscussionId");

                    b.HasIndex("ThemePollId");

                    b.HasIndex("ThemeTiebreakerPollId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Obskurnee.Models.Setting", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastChange")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Obskurnee.Models.StoredImage", b =>
                {
                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("FileContents")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Kind")
                        .HasColumnType("INTEGER");

                    b.HasKey("FileName");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Obskurnee.Models.Vote", b =>
                {
                    b.Property<string>("VoteId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PollId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostIdsSerialized")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("VoteId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PollId");

                    b.HasIndex("PostId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("PollPost", b =>
                {
                    b.Property<int>("AllPollsPollId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OptionsPostId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AllPollsPollId", "OptionsPostId");

                    b.HasIndex("OptionsPostId");

                    b.ToTable("PollPost");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Obskurnee.Models.Bookworm", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Obskurnee.Models.Bookworm", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Obskurnee.Models.Bookworm", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Obskurnee.Models.Bookworm", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Obskurnee.Models.Book", b =>
                {
                    b.HasOne("Obskurnee.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId");

                    b.HasOne("Obskurnee.Models.Round", "Round")
                        .WithOne("Book")
                        .HasForeignKey("Obskurnee.Models.Book", "RoundId");

                    b.Navigation("Post");

                    b.Navigation("Round");
                });

            modelBuilder.Entity("Obskurnee.Models.BookclubReview", b =>
                {
                    b.HasOne("Obskurnee.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Obskurnee.Models.Discussion", b =>
                {
                    b.HasOne("Obskurnee.Models.Round", "Round")
                        .WithMany("AllRelatedDiscussions")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Round");
                });

            modelBuilder.Entity("Obskurnee.Models.NewsletterSubscription", b =>
                {
                    b.HasOne("Obskurnee.Models.Bookworm", "User")
                        .WithMany("NewsletterSubscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Obskurnee.Models.Poll", b =>
                {
                    b.HasOne("Obskurnee.Models.Round", "Round")
                        .WithMany("AllRelatedPolls")
                        .HasForeignKey("RoundId");

                    b.Navigation("Round");
                });

            modelBuilder.Entity("Obskurnee.Models.Post", b =>
                {
                    b.HasOne("Obskurnee.Models.Discussion", "Discussion")
                        .WithMany("Posts")
                        .HasForeignKey("DiscussionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discussion");
                });

            modelBuilder.Entity("Obskurnee.Models.Round", b =>
                {
                    b.HasOne("Obskurnee.Models.Discussion", "BookDiscussion")
                        .WithMany()
                        .HasForeignKey("BookDiscussionId");

                    b.HasOne("Obskurnee.Models.Poll", "BookPoll")
                        .WithMany()
                        .HasForeignKey("BookPollId");

                    b.HasOne("Obskurnee.Models.Poll", "BookTiebreakerPoll")
                        .WithMany()
                        .HasForeignKey("BookTiebreakerPollId");

                    b.HasOne("Obskurnee.Models.Discussion", "ThemeDiscussion")
                        .WithMany()
                        .HasForeignKey("ThemeDiscussionId");

                    b.HasOne("Obskurnee.Models.Poll", "ThemePoll")
                        .WithMany()
                        .HasForeignKey("ThemePollId");

                    b.HasOne("Obskurnee.Models.Poll", "ThemeTiebreakerPoll")
                        .WithMany()
                        .HasForeignKey("ThemeTiebreakerPollId");

                    b.Navigation("BookDiscussion");

                    b.Navigation("BookPoll");

                    b.Navigation("BookTiebreakerPoll");

                    b.Navigation("ThemeDiscussion");

                    b.Navigation("ThemePoll");

                    b.Navigation("ThemeTiebreakerPoll");
                });

            modelBuilder.Entity("Obskurnee.Models.Vote", b =>
                {
                    b.HasOne("Obskurnee.Models.Poll", "Poll")
                        .WithMany()
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Obskurnee.Models.Post", null)
                        .WithMany("Votes")
                        .HasForeignKey("PostId");

                    b.Navigation("Poll");
                });

            modelBuilder.Entity("PollPost", b =>
                {
                    b.HasOne("Obskurnee.Models.Poll", null)
                        .WithMany()
                        .HasForeignKey("AllPollsPollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Obskurnee.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("OptionsPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Obskurnee.Models.Bookworm", b =>
                {
                    b.Navigation("NewsletterSubscriptions");
                });

            modelBuilder.Entity("Obskurnee.Models.Discussion", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Obskurnee.Models.Post", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("Obskurnee.Models.Round", b =>
                {
                    b.Navigation("AllRelatedDiscussions");

                    b.Navigation("AllRelatedPolls");

                    b.Navigation("Book")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
