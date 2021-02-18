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
        <input type="text" class="input" id="username" v-model="editingUser.name" placeholder="{{ editingUser.name }}" />
      </div>
      <div class="form-field">
        <label for="useremail" class="label">E-mail (nelze změnit):</label>
        <input type="email" readonly class="input readonly" id="useremail" v-model="editingUser.email" placeholder="{{ editingUser.email }}" />
      </div>
      <div class="form-field">
        <label for="userphone" class="label">Telefon:</label>
        <input type="tel" class="input" id="userphone" v-model="editingUser.phone" placeholder="{{ editingUser.phone }}" />
      </div>
      <div class="form-field">
        <label for="usergr" class="label">Profil na Goodreads:</label>
        <input type="url" class="input" id="usergr" v-model="editingUser.goodreadsUrl" placeholder="{{ editingUser.goodreadsUrl }}" />
      </div>
      <div class="form-field">
        <label for="userbio" class="label">Bio:</label>
        <textarea class="textarea" id="userbio" v-model="editingUser.aboutMe" placeholder="{{ editingUser.aboutMe }}"></textarea>
      </div>
    </div>
    <div class="profile-button">
      <a @click="updateProfile" class="button-primary button-margin" :v-if="isMod || user.userId == myUserId">Uložit změny</a>
      <a href="#" @click="stopEditing" class="button-secondary button-margin" :v-if="isMod || user.userId == myUserId">Zahodit změny</a>
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
        <div class="todo-l">☝🏻 not a fan of that. Radsej normalny link. Jeho text naozaj netreba ukazovat. </div>
      </div>
      <div class="profile-row">
        <div class="profile-cat">Bio:</div>
        <div class="profile-val" v-html="user.aboutMeHtml"></div>
      </div>
    </div>
    <div class="profile-button">
      <a @click="startEditing" class="button-primary" :v-if="isMod || user.userId == myUserId">Upravit údaje</a>
    </div>
  </div>


  <p class="todo-l"><strong>Laci:</strong>sem co este? Chceme tu naprikald userove reviews, recs?</p>
  <p class="todo"><strong>Rozárka:</strong>To bych nedávala do profilu, ten bych nechala čistě jako přehled osobních údajů určených k editaci. Review a recs bych nechala do sekce My.</p>
  <p class="todo-l"><strong>Laci:</strong>Okej, v tom pripade porozmyslaj nad rozlozenim stranok aj routes. Myslel som, ze budeme mat URL ako mame teraz, ze /my/mailadersa, kde budes vidiet skondenzovane user info a pod tym reviews a take veci. Iba mod by videl moznost 'edit'. <br/> Mozeme to dat aj inam, ale - kam? Ake budu URL?</p>
  <p class="todo"><strong>Rozárka:</strong>Budeme sem přidávat profile pic?</p>
  <p class="todo-l"><strong>Laci:</strong>Dobra otazka, co ja viem? Bude to niekto vyplnat?</p>
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
        editingUser: {},
      }
  },
  computed: {
    ...mapGetters("context", ["myUserId", "isMod"]),
  },
  methods: {
    ...mapActions("users", ["getUser", "updateUser"]),
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
      this.getUser(this.$route.params.email)
        .then(data => this.user = data);
    }
  },
  mounted() {
    this.fetchProfile();
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