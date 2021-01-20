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
    <button class="btn btn-primary ml-2" type="submit">Zaregistruj</button>
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
          
        }
      );
    },
  },
};
</script>