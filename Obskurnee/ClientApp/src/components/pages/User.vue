<template>
<section>
  <div v-if="user">
  
  <p v-if="mode == 'edit'">
    <input v-model="user.name" placeholder="meno" /><br />
    <input v-model="user.phone" placeholder="whatsapp telefon" /><br />
    <input v-model="user.goodreadsUrl" placeholder="goodreads Url profilu" /><br />
    <textarea v-model="user.aboutMe" placeholder="napis nieco o sebe _(markdown supported)_"></textarea><br />
    <button @click="updateProfile" class="button">Aktualizuj</button>
  </p>

  <div v-if="mode != 'edit'" class="todo">
    {{ user }}
    <a @click="mode = 'edit'" class="button" :v-if="isMod || user.userId == myUserId">EDITUJ</a>
  </div>


  <p class="todo">sem co este? Chceme tu naprikald userove reviews, recs?</p>
  </div>
</section>
</template>

<script>
import { mapActions, mapGetters } from "vuex";

export default {
  name: 'User',
  data() {
      return {
        user: {},
        mode: "default",
      }
  },
  computed: {
    ...mapGetters("context", ["myUserId", "isMod"]),
  },
  methods: {
    ...mapActions("users", ["getUser", "updateUser"]),
    updateProfile() {
      console.log(this.user);
      this.updateUser(this.user);
      this.mode = "default";
    }
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