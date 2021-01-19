<template>
<div>
  <form @submit.prevent="onSubmit">
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
</div>
</template>

<script>
import { mapActions, mapGetters, mapState } from "vuex";

export default {
  name: "Setup",
  data() {
    return {
      form: {
        email: "",
        password: "",
      },
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