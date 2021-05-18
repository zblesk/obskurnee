<template>
<section>
  <div v-if="user">
    <h1 class="page-title">{{ user.name }}</h1>

    <div v-if="mode == 'edit'">
      <div class="main">
        <div class="form-field">
          <label for="username">{{ $t('user.name')}} </label>
          <input type="text" id="username" required v-model="editingUser.name" tabindex="1" />
        </div>
        <div class="form-field">
          <label for="userphone">{{$t('user.phone')}}</label>
          <input type="tel" id="userphone" v-model="editingUser.phone" tabindex="2" />
        </div>
        <div class="form-field">
          <label for="usergr">{{$t('user.goodreadsProfile')}}</label>
          <input type="url" id="usergr" v-model="editingUser.goodreadsUrl" tabindex="3" />
        </div>
        <div class="form-field">
          <label for="userpic">{{$t('user.uploadAvatar')}}</label>
          <input type="file" id="userpic" name="userpic" accept="image/*" @change="onFilePicked" tabindex="4" />
        </div>
        <div class="form-field">
          <div class="label-md-wrapper">
            <label for="userbio" class="label-md">{{ $t('user.aboutMe') }}</label>
            <div class="mo-md">
              <div class="mo-md-pic">
                <img src="../../assets/Markdown-mark.svg" alt="markdown logo">
              </div>
              <div class="mo-md-link">
                <markdown-help-link tabindex="8"></markdown-help-link>
              </div>
            </div>
          </div>
          <textarea id="userbio" v-model="editingUser.aboutMe" :placeholder="$t('user.aboutMePlaceholder')" tabindex="5"></textarea>
        </div>
        <div class="buttons">
          <button @click="updateProfile" :disabled="saveInProgress" class="button-primary" :v-if="isMod || user.userId == myUserId" tabindex="6">{{ $t('menus.save') }}</button>
          <a @click="stopEditing" class="button-secondary button-cancel" :v-if="isMod || user.userId == myUserId" tabindex="7">{{ $t('menus.discardChanges') }}</a>
        </div>
      </div>
    </div>

    <div v-if="mode != 'edit'">
      <div class="main">
        <div v-if="user.avatarUrl" class="user-pic">
          <img  :src="user.avatarUrl" :title="user.name" :alt="user.name" />
          <p class="mod-text" v-if="user.isModerator">
            {{$t('global.mod')}}
            <span class="mod-tooltip">Tento uživatel je moderátor.</span>
          </p>
        </div>
        <div v-else class="user-pic-placeholder">
          <img src="../../assets/reader.svg"  :title="user.name" :alt="user.name" />
          <p class="mod-text" v-if="user.isModerator">
            {{$t('global.mod')}}
            <span class="mod-tooltip">Tento uživatel je moderátor.</span>
          </p>
        </div>

        <p v-if="user.aboutMeHtml" class="bio" v-html="user.aboutMeHtml"></p>
        <p v-else class="bio"><em>{{$t('user.noBioYet')}}</em></p>

        <div class="contacts">
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/email.svg" alt="email icon">
            </div>
            <div class="mo-text">
              <a v-if="user.email" :href="'mailto:' + user.email">{{ user.email }}</a>
              <p v-else>{{$t('user.noneAvailable')}}</p>
            </div>
          </div>
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/WhatsApp_Logo_1.png" alt="whatsapp logo">
            </div>
            <div class="mo-text">
              <a v-if="user.phone" :href="'tel:' + user.phone">{{ user.phone }}</a>
              <p v-else>{{$t('user.noneAvailable')}}</p>
            </div>
          </div>
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/goodreads_icon_32x32.png" alt="goodreads icon">
            </div>
            <div class="mo-text">
              <a v-if="user.goodreadsUrl" :href="user.goodreadsUrl">{{$t('user.goodreads')}}</a>
              <p v-else>{{$t('user.noneAvailable')}}</p>
            </div>
          </div>
        </div>

        <div class="buttons buttons-not-edit" v-if="isMod || user.userId == myUserId">
          <a @click="startEditing" class="button-primary" :v-if="isMod || user.userId == myUserId">{{$t('user.editInfo')}}</a>
          <a @click="$router.push({ name: 'passwordreset' })" class="button-secondary button-password" :v-if="isMod || user.userId == myUserId">{{$t('messages.resetPassword')}}</a>
        </div>

        <div class="reading" v-if="userHasCurrentlyReading(user.userId)">
          <h3 class="reading-title">{{$t('user.currentlyReading')}}</h3>
          <ul class="reading-list">
              <li v-for="review in usersCurrentlyReading(user.userId)" v-bind:key="review.ReviewId">
                <a :href="review.reviewUrl">{{ review.author }}: <strong>{{ review.bookTitle }}</strong></a>
              </li>
          </ul>
        </div>
        <div  class="reading" v-else>
          <h3 class="reading-title">{{$t('user.readingNothing')}}</h3>
        </div>

        <div class="languages" v-if="isMe(user.userId)">
          <h3 class="languages-title">{{$t('user.language')}}</h3>
          <language-selector v-if="isMe(user.userId)"></language-selector>
        </div>

        <div class="newletters" v-if="user && isMe(user.userId)">
          <h3 class="nl-title">{{$t('global.newsletters')}}</h3>
          <div class="nl-table">
            <div class="nl-row">
              <p class="nl-current">
                {{$t('global.basicEvents', 
                  [subscribedBasic 
                    ? $t('messages.subscribed') 
                    : $t('messages.notSubscribed')])}}
              </p>
              <div class="nl-button">
                <button @click="toggleSubscription('basicevents', !subscribedBasic)" class="button-primary button-small">{{ subscribedBasic ? $t('messages.unsubscribe') : $t('messages.subscribe') }}</button>
              </div>
            </div>
            <div class="nl-row">
              <p class="nl-current">
                  {{$t('global.allEvents', 
                    [subscribedAll 
                      ? $t('messages.subscribed') 
                      : $t('messages.notSubscribed')])}}
              </p>
              <div class="nl-button">
                <button @click="toggleSubscription('allevents', !subscribedAll)" 
                  class="button-primary button-small">
                    {{ subscribedAll ? $t('messages.unsubscribe') : $t('messages.subscribe') }}
                </button>
              </div>
            </div>
          </div>
          <i18n-t v-if="matrixRoomLink" keypath="user.matrixInfo" tag="p" class="nl-text">
              <a :href="'https://matrix.to/#/' + matrixRoomLink">{{ matrixRoomLink }}</a>
          </i18n-t>
        </div>
      </div>
    </div>

    <h2 class="page-subtitle page-subtitle--flex">
      <span>{{$t('user.recommends', [user.name])}}</span>
      <span v-if="user && isMe(user.userId)" class="recs-link"> 
        (<router-link :to="{ name: 'recommendationlist' }">{{$t('user.addRecommendation')}}</router-link>)
      </span>
      <img src="../../assets/download.svg" alt="toggle recommendations icon" 
        v-if="myRecs && myRecs.length > 0" @click="showRecs = !showRecs" 
        class="toggler" :class="{ 'toggler--hidden': !showRecs }">
      </h2>
    <div v-if="myRecs && myRecs.length > 0">
      <div v-if="showRecs">
        <div class="grid">
          <recommendation-card v-bind:recommendation="rec" :showName="false" 
            v-for="rec in myRecs" 
            v-bind:key="rec.recommendationId" />
        </div>
      </div>
    </div>
    <p v-else class="recs-message">{{$t('user.noRecsYet')}}
      <i18n-t v-if="user && isMe(user.userId)" tag="span" keypath="user.addFirstRec">
          <router-link :to="{ name: 'recommendationlist' }">{{$t('user.addFirstRecLinkText')}}</router-link>
      </i18n-t>
    </p>

    <h2 class="page-subtitle page-subtitle--flex u-mt-sp">
      <span>{{$t('user.booksRatedBy', [user.name])}}</span>
      <img src="../../assets/download.svg" alt="toggle reviews icon" 
        v-if="userReviews(user.userId)?.length > 0" @click="showReviews = !showReviews" 
        class="toggler" :class="{ 'toggler--hidden': !showReviews }">
    </h2>
    <div v-if="userReviews(user.userId)?.length > 0" class="reviews-wrapper">
      <div v-if="showReviews">
        <div class="grid">
          <users-review-card v-for="rev in userReviews(user.userId)" 
            v-bind:key="rev.reviewId" 
            v-bind:review="rev" ></users-review-card>
        </div>
      </div>
    </div>
    <p v-else class="recs-message">{{ $t('user.noneSoFar') }} 
      <span v-if="isMe(user.userId)">
        <router-link :to="{ name: 'booklist' }">{{ $t('user.wannaAddSome') }}</router-link>
      </span>
    </p>
  </div>
</section>
</template>

<script>
import { mapActions, mapGetters, mapState } from "vuex";
import axios from 'axios';
import RecommendationCard from '../RecommendationCard.vue';
import UsersReviewCard from '../UsersReviewCard.vue';
import LanguageSelector from '../LanguageSelector.vue';
import MarkdownHelpLink from '../MarkdownHelpLink.vue';
export default {
  name: 'User',
  components: { RecommendationCard, UsersReviewCard, LanguageSelector, MarkdownHelpLink },
  data() {
      return {
        user: {},
        mode: "default",
        editingUser: {},
        newAvatar: null,
        subscriptions: [],
        myRecs: [],
        saveInProgress: false,
        showRecs: true,
        showReviews: true
      }
  },
  computed: {
    ...mapGetters("context", ["myUserId", "isMod", "isMe"]),
    ...mapGetters("reviews", ["userReviews", "usersCurrentlyReading", "userHasCurrentlyReading"]),
    ...mapState("global", ["matrixRoomLink"]),
    subscribedBasic: function () { return this.subscriptions.includes('basicevents'); },
    subscribedAll: function () { return this.subscriptions.includes('allevents'); },
  },
  watch: {
    '$route' (to) {
      if (to.name == 'user')
      {
        this.onLoad();
      }
    }
  },
  methods: {
    ...mapActions("users", ["getUser", "updateUser", "updateAvatar"]),
    ...mapActions("recommendations", ["fetchRecommendationsFor"]),
    ...mapActions("reviews", ["fetchUserReviews", "fetchCurrentlyReading"]),
    async updateProfile() 
    {
      this.saveInProgress = true;
      if (this.newAvatar)
      {
        await this.updateAvatar(this.newAvatar)
          .catch(this.$handleApiError);
      }
      this.updateUser(this.editingUser)
        .then(() => {
          this.fetchProfile();
          this.stopEditing();
        })
        .catch(err => {
          this.saveInProgress = false;
          this.$handleApiError(err);
        });
    },
    startEditing() 
    {
      this.editingUser = JSON.parse(JSON.stringify(this.user));
      this.newAvatar = null;
      this.mode = "edit";
      this.saveInProgress = false;
    },
    stopEditing() 
    {
      this.mode = "default";
      this.saveInProgress = false;
    },
    fetchProfile()
    {
      return this.getUser(this.$route.params.email)
        .then(data => this.user = data)
        .catch(this.$handleApiError);
    },
    async fetchNewsletterSubsciptions()
    {
      return axios.get("/api/newsletters")
        .then((response) =>
        {
          this.subscriptions = response.data;
        })
        .catch(this.$handleApiError);
    },
    async toggleSubscription(newsletter, subscribe)
    {
      let action = subscribe ? "subscribe" : "unsubscribe";
        axios.post(`/api/newsletters/${newsletter}/${action}`)
          .then((response) =>
          {
            this.subscriptions = response.data;
          })
          .catch(this.$handleApiError);
    },
    onFilePicked (event) {
      const pic = event.target.files[0];
      this.newAvatar = new FormData();
      this.newAvatar.append("avatar", pic, pic.name);
    },
    onLoad()
    {
      this.fetchProfile()
        .then(() =>
        {
          if (this.$route.params.mode == "edit")
          {
            this.startEditing();
          }
          this.fetchRecommendationsFor(this.user.userId)
            .then(recs => this.myRecs = recs)
            .catch(this.$handleApiError);
          this.fetchUserReviews(this.user.userId)
            .then(data => this.reviews = data)
            .catch(this.$handleApiError);
          this.fetchCurrentlyReading()
            .catch(this.$handleApiError);
        });
      this.fetchNewsletterSubsciptions()
        .catch(this.$handleApiError);
    },
  },
  mounted() {
    this.onLoad();
  }
}
</script>

<style scoped>

  .button-cancel {
    margin-top: var(--spacer);
  }

  @media screen and (min-width: 576px) {
    .button-cancel {
      margin-top: 0;
      margin-left: var(--spacer);
    }
  }

  .bio {
    margin-bottom: calc(var(--spacer) * 2);
  }

  .contacts {
    margin-bottom: calc(var(--spacer) * 2);
  }

  .mo {
    display: flex;
    align-items: center;
    padding: 0;
    margin-bottom: calc(var(--spacer) / 2);
  }

  .mo-pic {
    width: 26px;
    flex-shrink: 0;
  }

  .mo-pic img {
    width: 100%;
    vertical-align: bottom;
  }

  .mo-text {
    margin: 0 0 0 calc(var(--spacer) / 2);
  }

  .mo-text a {
    text-decoration: none;
    font-weight: bold;
  }

  @media screen and (min-width: 768px) {
    .contacts {
      display: flex;
      justify-content: space-around;
    }

    .mo {
      margin-bottom: 0;
    }

    .mo:not(:last-child) {
      margin-right: calc(var(--spacer) * 2);
    }
  }

  .buttons-not-edit {
    margin-bottom: calc(var(--spacer) * 2);
  }

  .button-password {
    margin-top: var(--spacer);
  }

  @media screen and (min-width: 576px) {
    .button-password {
      margin-top: 0;
      margin-left: var(--spacer);
    }
  }

  .reading {
    margin-bottom: calc(var(--spacer) * 2);
  }

  .reading-title {
    margin-bottom: var(--spacer);
  }

  .reading a {
    text-decoration: none;
  }

  .reading-list {
    margin-bottom: 0;
  }

  .reading-list li {
    line-height: 1.5;
    margin-bottom: var(--spacer);
  }

  .reading-list li:last-child {
    margin-bottom: 0;
  }

  .button-small {
    padding: 0.3em 0.6em;
  }

  .languages {
    margin-bottom: calc(var(--spacer) * 2);
  }

  .languages-title {
    margin-bottom: var(--spacer);
  }

  .nl-table {
    margin-bottom: var(--spacer);
  }

  .nl-row {
    margin-top: 0;
    margin-bottom: 0;
    border-top: 1px solid var(--c-accent);
  }

  .nl-row:last-child {
    border-bottom: 1px solid var(--c-accent);
  }

  .nl-current {
    text-align: center;
    padding: calc(var(--spacer) / 2);
    margin-bottom: 0;
  }

  .nl-button {
    text-align: center;
    padding: 0 calc(var(--spacer) / 2) calc(var(--spacer) / 2) calc(var(--spacer) / 2);
  }

  @media screen and (min-width: 576px) {
    .nl-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      max-width: 400px;
    }

    .nl-button {
      padding-top: calc(var(--spacer) / 2);
    }
  }

  .page-subtitle {
    font-size: 1.5rem;
    text-align: center;
    margin-top: calc(var(--spacer) * 2);
    margin-bottom: var(--spacer) * 2;
    padding: 0 var(--spacer);
  }

  .recs-message {
    max-width: 800px;
    background-color: var(--c-bckgr-primary);
    margin: var(--spacer);
    padding: calc(2* var(--spacer));
  }

  @media screen and (min-width: 840px) {
    .recs-message {
      margin: var(--spacer) auto;
    }
  }

  .page-subtitle--flex {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    align-items: center;
  }

  .recs-link {
    font-weight: normal;
    font-size: 0.8em;
    margin-left: calc(var(--spacer) / 2);
  }

  .user-pic {
    width: 100px;
    height: 100px;
    margin: 0 auto var(--spacer) auto;
    border: 0;
    border-radius: 50%;
    position: relative;
  }

  .user-pic img {
    width: 100%;
    border-radius: 50%;
  }

  .user-pic-placeholder {
    width: 100px;
    height: 100px;
    margin: 0 auto var(--spacer) auto;
    padding-top: 20px;
    border: 0;
    border-radius: 50%;
    background-color: var(--c-accent);
    position: relative;
  }

  .user-pic-placeholder img {
    max-width: 60%;
    display: block;
    margin: 0 auto;
  }

  @media screen and (min-width: 576px) {
    .user-pic-placeholder,
    .user-pic {
      float: right;
      margin: 0 0 var(--spacer) var(--spacer);
    }
  }

  .mod-text {
    color: var(--c-accent);
    font-variant: small-caps;
    font-size: 1.25rem;
    font-weight: bold;
    transform: rotate(40deg);
    position: absolute;
    top: -10px;
    right: -10px;
  }

  .mod-tooltip {
    visibility: hidden;
    position: absolute;
    top: 0px;
    right: -20px;
    z-index: 1;
    transform: rotate(-40deg);
    width: 12rem;

    color: white;
    background-color: var(--c-accent);
    font-variant: normal;
    font-size: 0.875rem;
    font-weight: normal;
    padding: 5px;
    border-radius: 10px;
  }

  .mod-text:hover .mod-tooltip {
    visibility: visible;
  }

  .toggler {
    width: 0.9em;
    margin-left: calc(var(--spacer) / 2);
    transition-property: transform;
    transition-duration: 0.2s;
  }

  .toggler--hidden {
    transform: rotate(-90deg);
  }

  .reviews-wrapper:empty {
    margin-bottom: var(--spacer);
  }

</style>
