<template>
<div>
  <h1 class="page-title">Vytvoření nového uživatele</h1>
  <form @submit.prevent="onSubmit" class="main">
    <div class="form-field">
      <label for="emailInput">E-mailová adresa</label>
      <input id="emailInput" type="email" v-model="form.email" required>
    </div>
    <div class="form-field">
      <label for="passwordInput">Heslo</label>
      <input id="passwordInput" type="password" v-model="form.password" required>
    </div>
    <button class="button-primary" type="submit">Zaregistrovat</button>
  </form>
</div>
</template>

<script>
import { mapActions } from "vuex";

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
  },
  methods: {
    ...mapActions("context", ["registerFirstAdmin"]),
    onSubmit() {
      this.registerFirstAdmin(this.form)
        .then(() => {
            this.$router.push({ name: "home" });
        }
      );
    },
  },
};
</script>

<style scoped>
  .form-field {
    margin-bottom: var(--spacer);
  }

  .form-field label {
    display: block;
    margin-bottom: calc(var(--spacer) / 2);
  }

  .form-field textarea {
    width: 100%;
    height: 15em;
    margin-bottom: calc(var(--spacer) / 2);
  }

  .form-field input {
    width: 100%;
    max-width: 380px;
    margin-bottom: calc(var(--spacer) / 2);
  }
</style>