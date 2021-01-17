<template>
  <span>
    <span v-if="!isAuthenticated">
      <span v-if="!showLoginForm">
        <button class="btn btn-primary float-right ml-2" @click="this.showLoginForm = true">login</button>
      </span>
      <form @submit.prevent="onSubmit" @reset.prevent="onCancel" v-if="showLoginForm">
          <input id="emailInput"
                  type="email"
                  v-model="form.email"
                  required
                  placeholder="tvoj@mail" />
          <input id="passwordInput"
                        type="password"
                        v-model="form.password"
                        required
                        placeholder="h3sl0" />
        <button class="btn btn-primary float-right ml-2" type="submit">Prihlás</button>
        <button class="btn btn-secondary float-right" type="reset">Hups tak ne</button>
      </form>
    </span>
    <span v-if="isAuthenticated">
      👥 {{profile.name}}
      <button class="btn btn-primary float-right ml-2" @click="onLogout">logout</button>
    </span>
  </span>
</template>

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