<template>
<section>
  <div v-if="user">
    <h1 class="page-title">{{ user.name }}</h1>

    <div v-if="mode == 'edit'">
      <div class="profile">
        <div class="form-field">
          <label for="username" class="label">Jméno (povinné):</label>
          <input type="text" class="input" id="username" required v-model="editingUser.name" />
        </div>
        <div class="form-field">
          <label for="userphone" class="label">Telefon:</label>
          <input type="tel" class="input" id="userphone" v-model="editingUser.phone" />
        </div>
        <div class="form-field">
          <label for="usergr" class="label">Profil na Goodreads:</label>
          <input type="url" class="input" id="usergr" v-model="editingUser.goodreadsUrl" />
        </div>
        <div class="form-field">
          <label for="userbio" class="label">O mne:</label> 
          <textarea class="textarea" id="userbio" v-model="editingUser.aboutMe" placeholder="Napis sem nieco o sebe! Ake mas rada zanre? Co su Tvoje oblubene knihy? Co naopak nemas rada? Nieco ine, co nam o sebe povies?

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
          <div>
            <a href="https://www.markdownguide.org/cheat-sheet/" target="_blank">
              <img src="../../assets/markdown-logo.svg" width="25" height="25"/>
                Pomoc s Markdownom
              </a>
          </div>
        </div>
      </div>
      <div class="profile-button">
        <a @click="updateProfile" class="button-primary button-margin" :v-if="isMod || user.userId == myUserId">Uložit změny</a>
        <a @click="stopEditing" class="button-secondary button-margin" :v-if="isMod || user.userId == myUserId">Zahodit změny</a>
      </div>
    </div>

    <div v-if="mode != 'edit'">
      <div class="profile">
        <p class="bio" v-html="user.aboutMeHtml"></p>

        <div class="contacts">
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/email.svg" alt="email icon">
            </div>
            <div class="mo-text">
              <a :href="'mailto:' + user.email">{{ user.email }}</a>
            </div>
          </div>
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/WhatsApp_Logo_1.png" alt="whatsapp logo">
            </div>
            <div class="mo-text">
              <a :href="'tel:' + user.phone">{{ user.phone }}</a>
            </div>
          </div>
          <div class="mo">
            <div class="mo-pic">
              <img src="../../assets/goodreads_icon_32x32.png" alt="goodreads icon">
            </div>
            <div class="mo-text">
              <a :href="user.goodreadsUrl">Goodreads</a>
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

      <div v-if="myRecs && myRecs.length > 0" class="todo-l">
        <div v-for="rec in myRecs" v-bind:key="rec.postId">
          {{ rec.title }} - {{ rec.author }}
        </div>
      </div>
      <div v-else>
        Zatiaľ od Teba nemáme žiadne odporúčania. <span v-if="user && isMe(user.userId)">Čo tak <router-link :to="{ name: 'recommendationlist' }">nejaké pridať?</router-link> 💖</span>
      </div>


  </div>
</section>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import axios from 'axios';
export default {
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
    subscribedBasic: function () { return this.subscriptions.includes('basicevents'); },
    subscribedAll: function () { return this.subscriptions.includes('allevents'); },
  },
  methods: {
    ...mapActions("users", ["getUser", "updateUser"]),
    ...mapActions("recommendations", ["fetchRecommendationsFor"]),
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
  },
  mounted() {
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
      });
    this.fetchNewsletterSubsciptions();
  }
}
</script>

<style scoped>

  .profile,
  .profile-button {
    max-width: 800px;
  }

  .profile {
    background-color: var(--c-bckgr-primary);
    margin: var(--spacer);
    padding: calc(2* var(--spacer));
    text-align: left;
  }

  .profile-button {
    margin: 0 var(--spacer);
    text-align: center;
  }

  @media screen and (min-width: 840px) {
    .profile {
      margin: var(--spacer) auto;
    }

    .profile-button {
      margin: 0 auto;
    }
  }

  .profile-row {
    padding: calc(0.5 * var(--spacer)) 0;
  }

  .form-field {
    margin-bottom: var(--spacer);
  }

  .profile-cat,
  .label {
    font-size: 1.2em;
    font-weight: bold;
  }

  .label {
    display: block;
  }

  .input {
    width: 100%;
    border: 0;
    background-color: var(--c-bckgr-secondary);
    padding: 0.5em;
  }

  .input,
  .textarea {
    outline-color: var(--c-accent-secondary);
  }

  .readonly {
    outline: none;
    color: rgba(41, 41, 41, 0.5); /* --c-font plus opacity */
  }

  .textarea {
    font-size: 1em;
    border: 0;
    background-color: var(--c-bckgr-secondary);
    padding: 0.5em 1em;
    width: 100%;
    height: 15em;
  }

  .button-margin {
    margin: calc(0.5 * var(--spacer));
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
      justify-content: flex-start;
    }

    .mo {
      margin-bottom: 0;
    }

    .mo:not(:last-child) {
      margin-right: calc(var(--spacer) * 2);
    }
  }

  .buttons {
    display: flex;
    flex-direction: column;
    text-align: center;
    margin-bottom: calc(var(--spacer) * 2);
  }

  .button-password {
    margin-top: var(--spacer);
  }

  @media screen and (min-width: 576px) {
    .buttons {
      flex-direction: row;
      justify-content: center;
    }

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


</style>