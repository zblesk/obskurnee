<template>
<section>
  <div v-if="user">

    <h1 class="page-title">Uživatelské údaje</h1>
  
  <!--
  <p v-if="mode == 'edit'">
    <input v-model="user.name" placeholder="meno" /><br />
    <input v-model="user.phone" placeholder="whatsapp telefon" /><br />
    <input v-model="user.goodreadsUrl" placeholder="goodreads Url profilu" /><br />
    <textarea v-model="user.aboutMe" placeholder="napis nieco o sebe _(markdown supported)_"></textarea><br />
    <button @click="updateProfile" class="button">Aktualizuj</button>
  </p>
  -->

  <div v-if="mode == 'edit'">
    <div class="profile">
      <div class="form-field">
        <label for="username" class="label">Jméno:</label>
        <input type="text" class="input" id="username" v-model="user.name" placeholder="{{ user.name }}" />
      </div>
      <div class="form-field">
        <label for="useremail" class="label">E-mail (nelze změnit):</label>
        <input type="email" readonly class="input readonly" id="useremail" v-model="user.email" placeholder="{{ user.email }}" />
      </div>
      <div class="form-field">
        <label for="userphone" class="label">Telefon:</label>
        <input type="tel" class="input" id="userphone" v-model="user.phone" placeholder="{{ user.phone }}" />
      </div>
      <div class="form-field">
        <label for="usergr" class="label">Profil na Goodreads:</label>
        <input type="url" class="input" id="usergr" v-model="user.goodreadsUrl" placeholder="{{ user.goodreadsUrl }}" />
      </div>
      <div class="form-field">
        <label for="userbio" class="label">Bio:</label>
        <textarea class="textarea" id="userbio" v-model="user.aboutMe" placeholder="{{ user.aboutMe }}"></textarea>
      </div>
    </div>
    <div class="profile-button">
      <a @click="updateProfile" class="button-primary button-margin" :v-if="isMod || user.userId == myUserId">Uložit změny</a>
      <a href="#" class="button-secondary button-margin" :v-if="isMod || user.userId == myUserId">Zahodit změny</a>
    </div>
  </div>

  <div v-if="mode != 'edit'">
    <!--
    {{ user }}
    <a @click="mode = 'edit'" class="button" :v-if="isMod || user.userId == myUserId">EDITUJ</a>
    -->
    <div class="profile">
      <div class="profil-row">
        <div class="profile-cat">Jméno:</div>
        <div class="profile-val">{{ user.name }}</div>
      </div>
      <div class="profile-row">
        <div class="profile-cat">E-mail:</div>
        <div class="profile-val">{{ user.email }}</div>
      </div>
      <div class="profile-row">
        <div class="profile-cat">Telefon:</div>
        <div class="profile-val">{{ user.phone }}</div>
      </div>
      <div class="profile-row">
        <div class="profile-cat">Profil na Goodreads:</div>
        <div class="profile-val">
          <a :href="user.goodreadsUrl">{{ user.goodreadsUrl }}</a>
        </div>
      </div>
      <div class="profile-row">
        <div class="profile-cat">Bio:</div>
        <div class="profile-val">{{ user.aboutMe }}</div>
      </div>
    </div>
    <div class="profile-button">
      <a @click="mode = 'edit'" class="button-primary" :v-if="isMod || user.userId == myUserId">Upravit údaje</a>
    </div>

  </div>


  <p class="todo"><strong>Laci:</strong>sem co este? Chceme tu naprikald userove reviews, recs?</p>
  <p class="todo"><strong>Rozárka:</strong>To bych nedávala do profilu, ten bych nechala čistě jako přehled osobních údajů určených k editaci. Review a recs bych nechala do sekce My.</p>
  <p class="todo"><strong>Rozárka:</strong>Budeme sem přidávat profile pic?</p>
  </div>
</section>
</template>

<script>
import { mapActions, mapGetters } from "vuex";

export default {
  name: 'User',
  data() {
      return {
        user: {},
        mode: "default",
      }
  },
  computed: {
    ...mapGetters("context", ["myUserId", "isMod"]),
  },
  methods: {
    ...mapActions("users", ["getUser", "updateUser"]),
    updateProfile() {
      console.log(this.user);
      this.updateUser(this.user);
      this.mode = "default";
    }
  },
  mounted() {
    this.getUser(this.$route.params.email)
      .then(data => this.user = data);
      if (this.$route.params.mode)
      {
        this.mode = this.$route.params.mode;
      }
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

</style>