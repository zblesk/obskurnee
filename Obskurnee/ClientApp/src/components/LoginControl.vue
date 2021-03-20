<template>
  <div class="login">
    <div v-if="!isAuthenticated" class="not-auth">
      <div v-if="!showLoginForm" class="form-not-shown">
        <button class="button-primary button-show" @click="this.showLoginForm = true">Přihlásit</button>
      </div>
      <form @submit.prevent="onSubmit" @keyup.enter="onSubmit" @reset.prevent="onCancel" v-if="showLoginForm" class="login-form">
        <div class="login-form-field">
          <label for="emailInput">E-mail</label>
          <input id="emailInput" type="email" v-model="form.email" required />
        </div>
        <div class="login-form-field">
          <label for="passwordInput">Heslo</label>
          <input id="passwordInput" type="password" v-model="form.password" required />
        </div>
        <div class="buttons">
          <button class="button-primary button-login" type="submit">Přihlásit</button>
          <button class="button-secondary button-reset" @click="goToReset">Zabudnuté heslo</button>
          <button class="button-secondary button-logout" type="reset">Zrušit</button>
        </div>
      </form>
    </div>
    <div v-if="isAuthenticated" class="is-auth">
      <div class="menu-item">
        <router-link :to="{ name: 'user', params: { email: profile.email } }">Ja</router-link>
      </div>
      <div class="menu-logout">
        <button class="button-secondary button-logout" @click="onLogout">Odhlásit</button>
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

  .buttons {
    display: flex;
    flex-direction: column;
    max-width: 280px;
  }

  .button-login,
  .button-reset {
    margin-bottom: var(--spacer);
  }

  .login-form {
    padding: calc(var(--spacer) / 2) var(--spacer);
  }

  .login-form-field {
    margin-bottom: var(--spacer);
  }

  .login-form-field label {
    display: block;
    margin-bottom: 0;
  }

  .login-form-field input {
    width: 100%;
    max-width: 20em;
  }

  @media screen and (min-width: 992px) {
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
      padding: 0;
    }

    .login-form-field {
      display: flex;
      justify-content: right;
      margin-bottom: 0;
      margin-right: var(--spacer);
    }

    .login-form-field label {
      margin-right: calc(var(--spacer) / 2);
      line-height: 40px;
    }

    .login-form-field input {
      width: 10em;
    }

    .buttons {
      flex-direction: row;
      max-width: 100%;
    }

    .button-login,
    .button-reset {
      margin-bottom: 0;
      margin-right: var(--spacer);
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
    onSubmit() 
    {
      this.login(this.form)
        .then(() => {
          this.$notifySuccess("Vitaj späť");
        }
      )
      .catch((err) => {
          this.$notifyError("Prihlásenie zlyhalo :( ");
          console.log(err);
        }
      );
    },
    onCancel() 
    {
      this.form = {};
      this.showLoginForm = false;
    },
    onLogout()
    {
      this.logout();
      this.$router.push({ name: "home" });
      this.showLoginForm = false;
    },
    goToReset()
    {
      this.showLoginForm = false;
      this.$router.push({ name: 'passwordreset' });
    }
  },
};
</script>