<template>
  <section>
    <div class="homepage">

      <div class="welcome">
        <div class="welcome-pic">
          <img src="../../assets/hi.svg" alt="hi icon">
        </div>
        <p class="welcome-text">Vitaj, <span class="welcome-name">{{ myProfile.name }}</span>!
          <span v-if="currentDiscussion"> Zbierame
            <router-link :to="{name: 'discussion', params: { discussionId: currentDiscussion.discussionId },}">nové návrhy</router-link>.
          </span>
          <span v-if="currentPoll"> Práve beží
            <router-link :to="{ name: 'poll', params: { pollId: currentPoll.pollId } }">hlasovanie</router-link>.
          </span>
        </p>
      </div>

      <div v-if="!isAuthenticated" class="not-auth"></div>

      <div class="page-bottom">

        <div class="page-optional" v-if="isAuthenticated && (userProfileIncomplete || noticeboardHtml)">

          <div v-if="isAuthenticated && userProfileIncomplete" class="profile-info">
            <div class="info-pic">
              <img src="../../assets/information.svg" alt="information bubble icon">
            </div>
            <div class="info-text">
              <p>Ještě nemáš vyplněný profil. <router-link :to="{name: 'user', params: { email: myProfile.email, mode: 'edit' },}">Prosím, doplň ho.</router-link></p>
              <p>Zbývá ti doplnit:</p>
              <ul>
                <li v-if="!myProfile.name">jméno</li>
                <li v-if="!myProfile.phone">telefon (kontakt na whatsapp)</li>
                <li v-if="!myProfile.aboutMe">bio</li>
                <li v-if="!myProfile.goodreadsUrl">odkaz na tvůj profil na Goodreads</li>
              </ul>
            </div>
          </div>
          
          <div v-if="isAuthenticated && noticeboardHtml" v-html="noticeboardHtml" class="notice-board"></div>

        </div>

        <div class="blc-wrapper" v-if="currentBook && currentBook.post">
          <h2 class="blc-heading">Momentálne čítame knihu #{{ currentBook.order }}</h2>
          <book-large-card :post="currentBook.post" v-if="currentBook && currentBook.post"></book-large-card>
        </div>

      </div>

    </div>
  </section>
</template>

<script>
import axios from 'axios';
import BookLargeCard from '../BookLargeCard.vue';
import { mapGetters } from 'vuex';

export default {
  components: { BookLargeCard },
  name: 'Home',
  data() {
    return {
      currentBook: {},
      myProfile: {},
      noticeboardHtml: '',
      currentPoll: null,
      currentDiscussion: null,
    };
  },
  methods: {
    getBooks() {
      axios
        .get('/api/home')
        .then((response) => {
          if (response.data.books.length) {
            this.currentBook = response.data.books.shift();
          }
          this.noticeboardHtml = response.data.notice;
          this.myProfile = response.data.myProfile;
          this.currentPoll = response.data.currentPoll;
          this.currentDiscussion = response.data.currentDiscussion;
        })
        .catch(function(error) {
          console.log(error);
        });
    },
  },
  computed: {
    ...mapGetters('context', ['isAuthenticated']),
    userProfileIncomplete: function() {
      return (
        this.myProfile &&
        !(
          this.myProfile.name &&
          this.myProfile.phone &&
          this.myProfile.aboutMe &&
          this.myProfile.goodreadsUrl
        )
      );
    },
  },
  mounted() {
    this.getBooks();
  },
};
</script>

<style scoped>

  .not-auth {
    width: 100%;
    height: calc(var(--spacer) * 2);
  }

  .blc-wrapper {
    max-width: 800px;
    margin: calc(var(--spacer) * 2) var(--spacer);
    background-color: var(--c-bckgr-primary);
    overflow: hidden;
  }

  .welcome,
  .profile-info,
  .notice-board {
    max-width: 800px;
    margin: calc(var(--spacer) * 2) var(--spacer);
    padding: var(--spacer);
    background-color: var(--c-bckgr-primary);
    line-height: 1.5;
  }

  @media screen and (min-width: 840px) {
    .blc-wrapper,
    .welcome,
    .profile-info,
    .notice-board {
      margin: calc(var(--spacer) * 2) auto;
    }
  }

  @media screen and (min-width: 1200px) {
    .page-bottom {
      display: flex;
      flex-direction: row-reverse;

      max-width: 1680px;

      padding-left: var(--spacer);
      padding-right: var(--spacer);
      margin: 0 auto calc(var(--spacer) * 2) auto;
    }

    .page-optional {
      margin-left: calc(var(--spacer) * 2);

      flex-grow: 1;
      flex-basis: 100%;

      display: flex;
      flex-direction: column;
    }

    .notice-board {
      flex-grow: 100;
      width: 100%;
    }

    .profile-info {
      flex-grow: 1;
      width: 100%;
    }

    .blc-wrapper {
      margin: 0 auto;
      flex-grow: 1;
      flex-basis: 100%;
    }

    .notice-board {
      margin-top: 0;
      margin-bottom: 0;
    }

    .profile-info {
      margin-top: 0;
    }

  }

  .blc-heading {
    font-size: 1.2em;
    text-align: center;
    margin: var(--spacer) var(--spacer) 0 var(--spacer);
  }

  .welcome-name {
    font-weight: bold;
  }

  .welcome {
    display: flex;
    align-items: center;
  }

  .welcome-pic {
    width: 40px;
    flex-shrink: 0;
  }

  .welcome-pic img {
    width: 100%;
  }

  .welcome-text {
    margin: 0 0 0 var(--spacer);
  }

  .profile-info {
    display: flex;
    align-items: flex-start;
  }

  .info-pic {
    width: 40px;
    flex-shrink: 0;
  }

  .info-pic img {
    width: 100%;
  }

  .info-text {
    margin: 0 0 0 var(--spacer);
  }

  .info-text ul {
    margin-bottom: 0;
  }

  .info-text ul li:not(:last-child) {
    margin-bottom: calc(var(--spacer) / 2);
  }

  .info-text ul li:last-child {
    margin-bottom: 0;
  }




</style>
