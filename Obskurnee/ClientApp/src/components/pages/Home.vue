<template>
  <section>
    <div class="homepage">
      <div v-if="isAuthenticated" class="welcome">
        <div class="welcome-pic">
          <img src="../../assets/hi.svg" alt="null">
        </div>
        <p class="welcome-text">
          <i18n-t v-if="myProfile" tag="span" keypath="home.greeting">
            <strong>{{ myProfile.name }}</strong>
          </i18n-t>&nbsp;
          <i18n-t tag="span" v-if="currentDiscussion" keypath="home.ongoingDiscussion"> 
            <router-link :to="{name: 'discussion', params: { discussionId: currentDiscussion.discussionId },}">{{$t('home.ongoingDiscussionPosts')}}</router-link>&nbsp;
          </i18n-t>
          <i18n-t tag="span" v-if="currentPoll" keypath="home.ongoingPoll"> 
            <router-link :to="{ name: 'poll', params: { pollId: currentPoll.pollId } }">{{$t('home.ongoingPollVote')}}</router-link> 
          </i18n-t>
        </p>
      </div>

      <div v-if="!isAuthenticated" class="not-auth"></div>

      <div class="page-bottom">

        <div class="page-optional" v-if="isAuthenticated && (userProfileIncomplete || noticeboardHtml)">

          <div v-if="isAuthenticated && userProfileIncomplete" class="profile-info">
            <div class="info-pic">
              <img src="../../assets/information.svg" alt="null">
            </div>
            <div class="info-text">
              <i18n-t tag="p" keypath="home.profileInfoMissing">
                <router-link :to="{name: 'user', params: { email: myProfile.email, mode: 'edit' },}">{{$t('home.pleaseFillInfo')}}</router-link>
              </i18n-t>
              <p>{{$t('home.stillMissing')}}</p>
              <ul>
                <li v-if="!myProfile.name">{{$t('home.mName')}}</li>
                <li v-if="!myProfile.phone">{{$t('home.mPhone')}}</li>
                <li v-if="!myProfile.aboutMe">{{$t('home.mBio')}}</li>
                <li v-if="!myProfile.goodreadsUrl">{{$t('home.mGoodreads')}}</li>
              </ul>
            </div>
          </div>
          
          <div v-if="isAuthenticated && noticeboardHtml" v-html="noticeboardHtml" class="notice-board"></div>

        </div>

        <div class="blc-wrapper" v-if="currentBook && currentBook.post">
          <h2 class="blc-heading">{{$t('home.currentlyReadingBook', [currentBook.order])}}</h2>
          <book-large-card :book="currentBook" v-if="currentBook && currentBook.post"></book-large-card>
        </div>

      </div>

    </div>
  </section>
</template>

<script>
import BookLargeCard from '../BookLargeCard.vue';
import { mapGetters, mapState, mapActions } from 'vuex';

export default {
  components: { BookLargeCard },
  name: 'Home',
  methods: {
    ...mapActions('global', ['loadHome'])
  },
  computed: {
    ...mapGetters('context', ['isAuthenticated']),
    ...mapGetters('global', ['getLanguage']),
    ...mapState('global', ['myProfile', 'currentBook', 'noticeboardHtml', 'currentPoll', 'currentDiscussion']),
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
    this.loadHome()
      .then(() => {
        if (this.getLanguage)
        {
          this.$i18n.locale = this.getLanguage;
        }
      });
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

    .page-bottom > div + div {
      margin-right: calc(var(--spacer) * 2);
    }

    .page-optional {
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
      margin-bottom: 0;
    }

    .page-optional > div + div {
      margin-top: calc(var(--spacer) * 2);
    }

  }

  .blc-heading {
    font-size: 1.2rem;
    text-align: center;
    margin: var(--spacer) var(--spacer) 0 var(--spacer);
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
