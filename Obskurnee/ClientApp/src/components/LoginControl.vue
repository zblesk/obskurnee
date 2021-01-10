<template>
    <form @submit.prevent="onSubmit" @reset.prevent="onCancel">
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
</template>

<script>
import { mapActions } from "vuex";
export default {
  name: "LoginControl",
  data() {
    return {
      form: {
        email: "",
        password: "",
      },
    };
  },
  methods: {
    ...mapActions("context", ["login"]),
    onSubmit() {
      this.login({ authMethod: 'cookie', creds: this.form })
        .then(() => {
        }
      );
    },
    onCancel() {
      console.log('cancelthen',this.$refs);
      this.form = {};
    },
    onHidden() {
      Object.assign(this.form, {
        email: "",
        password: "",
      });
    },
  },
};
</script>