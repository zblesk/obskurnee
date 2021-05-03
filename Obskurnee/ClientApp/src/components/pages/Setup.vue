<template>
<div>
  <h1 class="page-title">{{$t('setup.userCreation')}}</h1>
  <form @submit.prevent="onSubmit" class="main">
    <div class="form-field">
      <label for="emailInput">{{$t('menus.email')}}</label>
      <input id="emailInput" type="email" v-model="form.email" required>
    </div>
    <div class="form-field">
      <label for="passwordInput">{{$t('menus.password')}}</label>
      <input id="passwordInput" type="password" v-model="form.password" required>
    </div>
    <button class="button-primary" type="submit">{{$t('menus.register')}}</button>
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
  .form-field input {
    max-width: 380px;
  }
</style>