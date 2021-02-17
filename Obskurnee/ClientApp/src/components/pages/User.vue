<template>
<section>
  <div v-if="user">

    <h1 class="page-title">Uživatelské údaje</h1>
  
  <p v-if="mode == 'edit'">
    <input v-model="user.name" placeholder="meno" /><br />
    <input v-model="user.phone" placeholder="whatsapp telefon" /><br />
    <input v-model="user.goodreadsUrl" placeholder="goodreads Url profilu" /><br />
    <textarea v-model="user.aboutMe" placeholder="napis nieco o sebe _(markdown supported)_"></textarea><br />
    <button @click="updateProfile" class="button">Aktualizuj</button>
  </p>

  <div v-if="mode != 'edit'">
    <!--
    {{ user }}
    <a @click="mode = 'edit'" class="button" :v-if="isMod || user.userId == myUserId">EDITUJ</a>
    -->
    <div class="profile">
      <div class="profile__row">
        <div class="profile__cat">Jméno:</div>
        <div class="profile__val">{{ user.name }}</div>
      </div>
      <div class="profile__row">
        <div class="profile__cat">E-mail:</div>
        <div class="profile__val">{{ user.email }}</div>
      </div>
      <div class="profile__row">
        <div class="profile__cat">Telefon:</div>
        <div class="profile__val">{{ user.phone }}</div>
      </div>
      <div class="profile__row">
        <div class="profile__cat">Můj profil na Goodreads:</div>
        <div class="profile__val">
          <a href="{{ user.goodreadsUrl }}">
            {{ user.goodreadsUrl }}
          </a>
        </div>
      </div>
      <div class="profile__row">
        <div class="profile__cat">Bio:</div>
        <div class="profile__val">{{ user.aboutMe }}</div>
      </div>
    </div>
    <div class="profile-button">
      <a @click="mode = 'edit'" class="button__primary" :v-if="isMod || user.userId == myUserId">Upravit údaje</a>
    </div>

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

<style scoped>

  .profile {
    max-width: 600px;
    background-color: #FFFFFF;
    margin: 30px auto 30px auto;
    padding: 40px;
    text-align: left;
  }

  .profile__row {
    padding: 10px 0 10px 0;
  }

  .profile__cat {
    font-weight: bold;
  }

  .profile-button {
    max-width: 600px;
    margin: 0 auto;
    text-align: center;
  }


</style>