# Obskurnee - a companion for your Book club

![logo](./logo.png)

[![Build Status](https://bzzz.zble.sk/api/badges/zblesk/Obskurnee/status.svg)](https://bzzz.zble.sk/zblesk/Obskurnee)

**Do you have a book club? Are you looking for an app that would let you suggest books to one another, then vote for the ones you're going to read next? Would you like Goodreads integration? Then Obskurnee might be for you.**

Obskurnee is a simple, user-friendly web app that will help you decide on your book club's next read. It will also let you add book recommendations when you've read something you Just Have To Tell Everyone About. You can review the books your Book club has read and share the ones you're currently reading (via Goodreads). But not much else. We've kept the feature set small by design - we wanted to implement the workflow we use, not cover every possible use-case.

Our aim was to make the app user friendly and intuitive to use. 

>  ℹ Keep in mind: there is no 'central service' where you could register and just start using the app, at least not for now. You have to run the web app yourself. If you just want to check it out, have a look at the [demo](https://obskurnee.zblesk.net/). Simply log in with any made-up email address and password.

>  ℹ The [GitHub](https://github.com/zblesk/obskurnee) repository is a mirror. 

>  ℹ Some features you might expect from an app are still missing. Since our free time is limited, we made a polished MVP we were able to start using straight away; we expect to be adding more in the future, but it might take a while.

**Available in English and Slovak**.

If you want to chat, feel free to join Obskurnee's Matrix chat room at [#obskurnee:zble.sk](https://matrix.to/#/#obskurnee:zble.sk).

# Features and screenshots

The way Obskurnee works is simple: When your book club is ready for a new book, a moderator opens a new voting round. **All members can add any number of suggestions. Once a moderator closes the round, a poll is automatically generated. When all members have voted, the poll is evaluated and the chosen book is featured on the home page.**

Aside from the current book, the home page features a global notice board (empty and thus hidden on this screenshot), and ongoing activities, such as voting.

![Home page](./Screens/home.png)

A shelf for all the books you've read together. Users can **rate and review** any of these books.

![All books](./Screens/shelf.png)

Voting rounds offer two options: either a direct vote for books or the choice of a round with specific topics.

![Voting rounds](./Screens/voting_rounds.png)

The **topic** option entails a vote in two stages. First, the members suggest book topics (such as a particular genre, year of publication, nationality of author etc.), and once the winning theme is selected, a new round for **books** related to this theme is automatically opened.

The easiest way to add a book is to copy and paste a Goodreads URL and wait for it to be scraped, with the option to add further comments. All large text fields support **Markdown**. Everything you'd expect from MD should be there; plus, you can use `::this::` to mark a spoiler.

![Adding a book](./Screens/adding_book.gif)



**Recommendations** work in the same way as the books suggested for voting, except they're not tied to a specific round. Use these as general recommendations for other members of the book club.

Only **moderators can add new users** (though this is disabled in the demo). Once logged in, **new users are prompted to fill out their profiles** - especially their Goodreads profile (which is also used to **load their "Currently Reading" shelf**), phone number (since we mainly communicate over Whatsapp), and a brief bio about themselves and/or their reading preferences. Each user's page also shows their reviews and recommendations.

# [Try the demo](https://obskurnee.zblesk.net/)

**Enter *any* email and password.**

# Setup and tech info

To try it out, run `docker run -p 8080:8080 -e DefaultCulture=en zblesk/obskurnee`.

Then navigate to http://localhost:8080/setup and create the first user.

More complete setup info to come later.

# Development 

You will need .NET 5.0 and Node (we run on 14.x).

1. Clone repo
2. `cd .\Obskurnee\ClientApp\`
3. `npm install`
4. `cd ..`
5. `dotnet run`

That should let you access your local copy at http://localhost:5000. 

# License

Copyright (c) 2021 Ladislav Benc

Personal Use License

Obskurnee is available for personal use only. Feel free to run it for yourself and/or your friends on your own server, for non-commercial purposes only. 

Software is provided without any guarantees. Use at your own risk. 