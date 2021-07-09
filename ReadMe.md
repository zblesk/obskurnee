# Obskurnee - a companion for your Book club

![logo](./logo.png)

[![Build Status](https://bzzz.zble.sk/api/badges/zblesk/Obskurnee/status.svg)](https://bzzz.zble.sk/zblesk/Obskurnee)

**Do you have a book club? Are you looking for an app that would let you pitch books to one another, then vote on the one you're going to read? Would you like Goodreads integration? Then Obskurnee might be for you.**

Obskurnee is a simple, user-friendly web app that will help you decide on your book club's next read. It will also let you write book recommendations, when you've read something you Just Have To Tell Everyone About. You can also review books the Book club has read, and share the books you're currently reading (via Goodreads). But not much else. We've kept the feature set small by design - we wanted to implement the workflow that we use, not cover every possible use-case. 

Our aim was to make the app user friendly and intuitive to use. 

>  ℹ Keep in mind: there is no 'central service' where you could just register and just start using, for now. You have to run the web app yourself. If you just want to check it out, look at the [demo](https://obskurnee.zble.sk/). Make up any mail and password, and you will log in successfully.

>  ℹ The [GitHub](https://github.com/zblesk/obskurnee) repository is a mirror. 

>  ℹ Some features you might expect from an app are still missing. Since our free time is limited, we made a polished MVP we can start using; we expect to add more in the future, but it might not be quick.

**Available in English and Slovak**.

If you want to chat, feel free to join Obskurnee's Matrix chat room at [#obskurnee:zble.sk](https://matrix.to/#/#obskurnee:zble.sk).

# Features and screenshots

The way Obskurnee works is simple: When your book club is ready for a new book, a moderator starts a new voting round. **All members can pitch in their suggestions. Then the moderator closes the round and a poll is generated. When all members have voted, the poll is evaluated and the victorious book takes the most prominent spot on the home page.**

Besides the current book, the home page features the global notice board (empty and thus hidden on this screenshot), and ongoing activities such as voting.

![Home page](./Screens/home.png)

A shelf for all the books you've read together. Users can write **reviews** for any of these books.

![All books](./Screens/shelf.png)

Voting rounds. When starting a new one, the moderator can choose whether the members will pitch books directly, or if a topic for the upcoming round should be voted upon, first. 

![Voting rounds](./Screens/voting_rounds.png)

When stating with **Topics**, the flow is similar: first people pitch topic suggestions, then one winner is selected through voting, then people pitch **Books** for that topic. 

The easiest way to add a book is to first copy a Goodreads URL and wait for it to be scraped; then just adding your own comments. All large text fields support **Markdown**. Everything you'd expect from MD should be there; plus, you can use `::this::` to mark a spoiler.

![Adding a book](./Screens/adding_book.gif)



**Recommendations** are just like pitching a book, except they're not tied to a specific round. Write one if you want to recommend a book to anyone in general. 

Only **moderators can add new users** (though this is disabled in the demo). After logging in, new **users are prompted to fill out their profiles** - especially their Goodreads profile (which is also used to **load their "Currently Reading" shelf**), phone number (since we mainly communicate over Whatsapp), and a few words about themselves and/or their reading preferences. Each user's page also shows their reviews and recommendations.

# Try the demo

sorry, demo not read yet. Hopefully, soon.

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