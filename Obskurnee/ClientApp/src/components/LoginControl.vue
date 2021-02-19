<template>
  <div class="login">
    <div v-if="!isAuthenticated" class="not-auth">
      <div v-if="!showLoginForm" class="form-not-shown">
        <button class="button-show" @click="this.showLoginForm = true">Přihlášení</button>
      </div>
      <form @submit.prevent="onSubmit" @reset.prevent="onCancel" v-if="showLoginForm" class="login-form">
        <div class="login-form-field">
          <label for="emailInput">E-mail</label>
          <input id="emailInput" type="email" v-model="form.email" required />
        </div>
        <div class="login-form-field">
          <label for="passwordInput">Heslo</label>
          <input id="passwordInput" type="password" v-model="form.password" required />
        </div>
        <button class="button-login" type="submit">Přihlásit</button>
        <button class="button-reset" type="reset">Zrušit</button>
      </form>
    </div>
    <div v-if="isAuthenticated" class="is-auth">
      <div class="menu-item">
        <router-link :to="{ name: 'user', params: { email: profile.email } }">Já</router-link>
      </div>
      <div class="menu-logout">
        <button class="button-logout" @click="onLogout">Odhlásit</button>
      </div>
    </div>
  </div>
</template>

<style scoped>

  .menu-item,
  .menu-logout,
  .form-not-shown {
    height: 40px;
    line-height: 40px;
    padding-left: var(--spacer);
  }

  .menu-item:hover {
    background-color: var(--c-accent);
  }

  .menu-item:hover a {
    color: var(--c-font-rev);
  }

  .menu-item a {
    font-size: 1.2em;
    color: var(--c-font);
    text-decoration: none;
  }

  .button-logout,
  .button-login,
  .button-show,
  .button-reset {
    border-radius: 4px;
    font-size: 1em;
    line-height: 1em;
    font-weight: bold;
    padding: 10px 10px;
    cursor: pointer;
    display: inline-block;
  }

  .button-logout,
  .button-reset {
    border: 2px solid var(--c-accent-secondary);
    background-color:var(--c-bckgr-primary);
    color: var(--c-accent-secondary);
  }

  .button-logout:hover,
  .button-reset:hover {
    color: var(--c-font-rev);
    text-decoration: none;
    background-color: var(--c-accent-secondary);
  }

  .button-login,
  .button-show {
    border: 2px solid var(--c-accent);
    background-color: var(--c-accent);
    color: var(--c-font-rev);
  }

  .button-login {
    margin-right: var(--spacer);
  }

  .button-login:hover,
  .button-show:hover {
    color: var(--c-font-rev);
    text-decoration: none;
    background-color: var(--c-accent-darker);
    border-color: var(--c-accent-darker);
  }

  .login-form {
    padding-left: var(--spacer);
    padding-top: calc(var(--spacer) / 2);
  }

  .login-form-field {
    margin-bottom: var(--spacer);
    margin-right: var(--spacer);
  }

  .login-form-field label {
    display: block;
    margin-bottom: 0;
  }

  .login-form-field input {
    width: 100%;
    max-width: 20em;
  }

  @media screen and (min-width: 700px) {
    .is-auth {
      display: flex;
      justify-content: space-between;
    }

    .menu-item {
      padding-left: calc(var(--spacer) / 2);
      padding-right: calc(var(--spacer) / 2);
    }
  
    .login-form {
      display: flex;
      justify-content: right;
      padding-left: 0;
      padding-top: 0;
    }

    .login-form-field {
      display: flex;
      justify-content: right;
      margin-bottom: 0;
    }

    .login-form-field label {
      margin-right: calc(var(--spacer) / 2);
      line-height: 40px;
    }

    .login-form-field input {
      width: 8em;
    }
  }

  @media screen and (min-width: 815px) {
    .login-form-field input {
      width: 12em;
    }
  }

  @media screen and (min-width: 915px) {
    .login-form-field input {
      width: 15em;
    }
  }

</style>

<script>
import { mapActions, mapGetters, mapState } from "vuex";

export default {
  name: "LoginControl",
  data() {
    return {
      form: {
        email: "",
        password: "",
      },
      showLoginForm: false,
    };
  },
  computed: {
    ...mapState("context", ["profile"]),
    ...mapGetters("context", ["isAuthenticated"])
  },
  methods: {
    ...mapActions("context", ["login", "logout"]),
    onSubmit() {
      this.login(this.form)
        .then(() => {
        }
      );
    },
    onCancel() {
      this.form = {};
      this.showLoginForm = false;
    },
    onLogout(){
      this.logout();
      this.$router.push({ name: "home" });
      this.showLoginForm = false;
    }
  },
};
</script>