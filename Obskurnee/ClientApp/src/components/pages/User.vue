<template>
<section>
  <div v-if="user">
  {{ user }}

  <p v-if="mode == 'edit'" class="todo">Som v EDITACNOM MODE!</p>
  <div v-if="mode != 'edit'" class="todo"><a @click="mode = 'edit'">Editkuj - KLIKNI NA MNA </a></div>


  <p class="todo">sem co este? Chceme tu naprikald userove reviews, recs?</p>
  </div>
</section>
</template>

<script>
import { mapActions } from "vuex";

export default {
  name: 'User',
  data() {
      return {
        user: {},
        mode: "default",
      }
  },
  methods: {
    ...mapActions("users", ["getUser"]),
  },
  mounted() {
    this.getUser(this.$route.params.email)
      .then(data => this.user = data);
      if (this.$route.params.mode)
      {
        this.mode = this.$route.params.mode;
      }
  }
}
</script>