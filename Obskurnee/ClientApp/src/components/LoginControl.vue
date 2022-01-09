<template>
  <div class="login">
    <div v-if="!isAuthenticated" class="not-auth">
      <div v-if="!showLoginForm" class="form-not-shown">
        <button class="button-primary button-show" @click="showLoginForm = true">{{ $t('menus.login') }}</button>
      </div>
      <form @submit.prevent="onSubmit" @keyup.enter="onSubmit" @reset.prevent="onCancel" v-if="showLoginForm" class="login-form">
        <div class="login-form-field">
          <label for="emailInput">{{ $t('menus.email') }}</label>
          <input id="emailInput" type="email" v-model="form.email" required />
        </div>
        <div class="login-form-field">
          <label for="passwordInput">{{ $t('menus.password') }}</label>
          <input id="passwordInput" type="password" v-model="form.password" required />
        </div>
        <div class="buttons">
          <button class="button-primary button-login" type="submit">{{ $t('menus.login') }}</button>
          <button class="button-secondary button-reset" @click="goToReset">{{ $t('menus.forgottenPwd') }}</button>
          <button class="button-secondary button-cancel" type="reset">{{ $t('menus.cancel') }}</button>
        </div>
      </form>
    </div>
    <div v-if="isAuthenticated" class="is-auth">
      <router-link :to="{ name: 'user', params: { email: profile.email } }">
        <div class="menu-item">{{ $t('menus.me') }}</div>
      </router-link>
      <div class="menu-logout">
        <button class="button-secondary button-logout" @click="onLogout">{{ $t('menus.logout') }}</button>
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
    font-size: 1.2em;
    color: var(--c-font);
  }

  .menu-item:hover,
  .menu-item:focus,
  .menu-item:active {
    background-color: var(--c-accent);
    color: var(--c-font-rev);
  }

  .is-auth a {
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

  .button-show,
  .button-login,
  .button-reset,
  .button-cancel,
  .button-logout {
    font-size: 16px;
  }

  .login-form {
    padding: calc(var(--spacer) / 2) var(--spacer);
  }

  .login-form-field {
    margin-bottom: var(--spacer);
  }

  .login-form-field label {
    display: block;
    font-size: 1rem;
    margin-bottom: calc(var(--spacer) / 3);
  }

  .login-form-field input {
    font-size: 0.875rem;
    border: 0;
    background-color: var(--c-bckgr-secondary);
    padding: calc(var(--spacer) / 2);
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
      margin-bottom: 0;
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
          if (this.$route.params.nextUrl != null) {
            this.$router.push(this.$route.params.nextUrl)
          }
        }
      )
      .catch((err) => {
          this.$notifyError(this.$t('messages.loginFailed'));
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