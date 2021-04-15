<template>
<section>
  <div v-if="user">
    <div class="page-title-wrapper">
      <div class="mod" v-if="user.isModerator">
        <img src="../../assets/fairy.svg" alt="fairy icon">
      </div>
      <h1 class="page-title-user">{{ user.name }}</h1>
    </div>

    <div v-if="mode == 'edit'">
      <div class="main">
        <div class="form-field">
          <label for="username">Jméno (povinné):</label>
          <input type="text" id="username" required v-model="editingUser.name" />
        </div>
        <div class="form-field">
          <label for="userphone">Telefon:</label>
          <input type="tel" id="userphone" v-model="editingUser.phone" />
        </div>
        <div class="form-field">
          <label for="usergr">Profil na Goodreads:</label>
          <input type="url" id="usergr" v-model="editingUser.goodreadsUrl" />
        </div>
        <div class="form-field">
          <div class="label-wrapper">
            <label for="userbio" class="label-userbio">O mne:</label>
            <div class="mo mo-markdown">
              <div class="mo-pic">
                <img src="../../assets/Markdown-mark.svg" alt="markdown logo">
              </div>
              <div class="mo-markdown-link">
                <a href="https://www.markdownguide.org/cheat-sheet/">Pomoc s Markdownom</a>
              </div>
            </div>
          </div>
          <textarea id="userbio" v-model="editingUser.aboutMe" placeholder="Napis sem nieco o sebe! Ake mas rada zanre? Co su Tvoje oblubene knihy? Co naopak nemas rada? Nieco ine, co nam o sebe povies?

  Mozes pouzit Markdown na jednoduche formatovanie textu. 
  Ak by si sa stratila, klikni na help ikonku nizsie.
  Medzi zaklady patri napriklad: 

  # Najvacsi nadpis
  ## mensi nadpis 

  **tucny textt** alebo _kurziva_ 

  - necislovany
  - zoznam
  - je 
  - jednoduchy

  1. cislovany
  2. tiez
  3. lahky

  > takto sa pise citat
  > **moze** obsahovat aj _**formatovanie**_

  Mozes lahko pridat aj [link](https://google.sk)"></textarea>
        </div>
        <div class="buttons">
          <a @click="updateProfile" class="button-primary" :v-if="isMod || user.userId == myUserId">Uložit změny</a>
          <a @click="stopEditing" class="button-secondary button-cancel" :v-if="isMod || user.userId == myUserId">Zahodit změny</a>
        </div>
      </div>
    </div>

    <div v-if="mode != 'edit'">
      <div class="main">
        <p v-if="user.aboutMeHtml" class="bio" v-html="user.aboutMeHtml"></p>
        <p v-else class="bio"><em>Zatiaľ nám o sebe nič nepovedala.</em></p>

        <div class="contacts">
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/email.svg" alt="email icon">
            </div>
            <div class="mo-text">
              <a v-if="user.email" :href="'mailto:' + user.email">{{ user.email }}</a>
              <p v-else>Nemáme</p>
            </div>
          </div>
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/WhatsApp_Logo_1.png" alt="whatsapp logo">
            </div>
            <div class="mo-text">
              <a v-if="user.phone" :href="'tel:' + user.phone">{{ user.phone }}</a>
              <p v-else>Nemáme</p>
            </div>
          </div>
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/goodreads_icon_32x32.png" alt="goodreads icon">
            </div>
            <div class="mo-text">
              <a v-if="user.goodreadsUrl" :href="user.goodreadsUrl">Goodreads</a>
              <p v-else>Nemáme</p>
            </div>
          </div>
        </div>

        <div class="buttons" v-if="isMod || user.userId == myUserId">
          <a @click="startEditing" class="button-primary" :v-if="isMod || user.userId == myUserId">Upravit údaje</a>
          <a @click="$router.push({ name: 'passwordreset' })" class="button-secondary button-password" :v-if="isMod || user.userId == myUserId">Zmeniť heslo</a>
        </div>

        <div class="reading" v-if="user.currentlyReading?.length">
          <h3 class="reading-title">Právě čte:</h3>
          <ul class="reading-list">
              <li v-for="review in user.currentlyReading" v-bind:key="review.ReviewId"><a :href="review.reviewUrl">{{ review.author }}: <strong>{{ review.bookTitle }}</strong></a></li>
          </ul>
        </div>
        <div  class="reading" v-else>
          <h3 class="reading-title">Práve nič nečíta.</h3>
        </div>

        <div class="newletters" v-if="user && isMe(user.userId)">
          <h3 class="nl-title">Newsletters</h3>
          <div class="nl-table">
            <div class="nl-row">
              <p class="nl-current">Základné udalosti: {{ subscribedBasic ? 'Prihlásené' : 'Neprihlásené' }}</p>
              <div class="nl-button">
                <button @click="toggleSubscription('basicevents', !subscribedBasic)" class="button-primary button-small">{{ subscribedBasic ? 'Odhlásiť' : 'Prihlásiť' }}</button>
              </div>
            </div>
            <div class="nl-row">
              <p class="nl-current">Všetky udalosti: {{ subscribedAll ? 'Prihlásené' : 'Neprihlásené' }}</p>
              <div class="nl-button">
                <button @click="toggleSubscription('allevents', !subscribedAll)" class="button-primary button-small">{{ subscribedAll ? 'Odhlásiť' : 'Prihlásiť' }}</button>
              </div>
            </div>
          </div>
          <p class="nl-text">Všetky notifikácie tiež budú zaslané do chatroomu <a href="https://matrix.to/#/#bookclub:zble.sk">#bookclub:zble.sk</a><span class="todo-l">, ale zatial idu do inej, testovacej miestnosti nech v tej 'ostrej' nie je vyvojovy spam :D</span></p>
        </div>

      </div>
    </div>

    <h2 class="page-subtitle">{{ user.name }} doporučuje ke čtení</h2>
    <div v-if="myRecs && myRecs.length > 0" class="grid">
        <recommendation-card v-bind:post="rec" :showName="false" v-for="rec in myRecs" v-bind:key="rec.postId" />
    </div>
    <p v-else class="recs-message">Zatiaľ tu nemáme žiadne odporúčania. <span v-if="user && isMe(user.userId)">Čo tak <router-link :to="{ name: 'recommendationlist' }">nejaké pridať?</router-link></span></p>

    <h2 v-if="userReviews(user.userId)" class="page-subtitle u-mt">Knihy, které {{ user.name }} hodnotila</h2>
    <div v-if="userReviews(user.userId)" class="grid">
      <users-review-card v-for="rev in userReviews(user.userId)" v-bind:key="rev.reviewId" v-bind:review="rev" ></users-review-card>
    </div>
  </div>
</section>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import axios from 'axios';
import RecommendationCard from '../RecommendationCard.vue';
import UsersReviewCard from '../UsersReviewCard.vue';
export default {
  components: { RecommendationCard, UsersReviewCard },
  name: 'User',
  data() {
      return {
        user: {},
        mode: "default",
        editingUser: {},
        subscriptions: [],
        myRecs: [],
      }
  },
  computed: {
    ...mapGetters("context", ["myUserId", "isMod", "isMe"]),
    ...mapGetters("reviews", ["userReviews"]),
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
    ...mapActions("users", ["getUser", "updateUser"]),
    ...mapActions("recommendations", ["fetchRecommendationsFor"]),
    ...mapActions("reviews", ["fetchUserReviews"]),
    updateProfile() 
    {
      this.updateUser(this.editingUser)
        .then(() => {
          this.fetchProfile();
          this.stopEditing();
        });
    },
    startEditing() 
    {
      this.editingUser = JSON.parse(JSON.stringify(this.user));
      this.mode = "edit";
    },
    stopEditing() 
    {
      this.mode = "default";
    },
    fetchProfile()
    {
      return this.getUser(this.$route.params.email)
        .then(data => this.user = data);
    },
    async fetchNewsletterSubsciptions()
    {
      return axios.get("/api/newsletters")
        .then((response) =>
        {
          this.subscriptions = response.data;
        })
        .catch(function (error)
        {
          this.$notifyError(error);
        });
    },
    async toggleSubscription(newsletter, subscribe)
    {
      let action = subscribe ? "subscribe" : "unsubscribe";
        axios.post(`/api/newsletters/${newsletter}/${action}`)
          .then((response) =>
          {
            this.subscriptions = response.data;
          })
          .catch((err) => this.$notifyError(err));
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
            .catch((err) => this.$notifyError(err));
          this.fetchUserReviews(this.user.userId)
            .then(data => {
              this.reviews = data;
              console.log('b', data);
            })
            .catch(e => this.$notifyError(e));
        });
      this.fetchNewsletterSubsciptions();
    },
  },
  mounted() {
    this.onLoad();
  }
}
</script>

<style scoped>

  .page-title-wrapper {
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    align-items: center;
    margin-top: calc(1.5 * var(--spacer));
    margin-bottom: var(--spacer);
  }

  .mod {
    width: 36px;
    margin-right: var(--spacer);
  }

  .mod img {
    width: 100%;
  }

  .page-title-user {
    font-size: 2em;
    font-weight: bold;
    margin-bottom: 0;
  }

  .profile-row {
    padding: calc(0.5 * var(--spacer)) 0;
  }

  .form-field textarea {
    margin-top: calc(var(--spacer) / 2);
  }

  .label-wrapper {
    display: flex;
    flex-direction: column;
  }

  @media screen and (min-width: 576px) {
    .label-wrapper {
      flex-direction: row;
      justify-content: space-between;
    }

    .label-userbio {
      margin-bottom: 0;
    }
  }

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

  .mo-text,
  .mo-markdown-link {
    margin: 0 0 0 calc(var(--spacer) / 2);
  }

  .mo-text a {
    text-decoration: none;
    font-weight: bold;
  }

  .mo-markdown a {
    text-decoration: underline;
    font-weight: normal;
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

  .buttons {
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
    font-size: 1.5em;
    text-align: center;
    margin-top: calc(var(--spacer) * 2);
    margin-bottom: var(--spacer) * 2;
  }

  .u-mt {
    margin-top: var(--spacer);
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


</style>