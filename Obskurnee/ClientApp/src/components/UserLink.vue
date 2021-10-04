<template>
  <router-link :to="{ name: 'user', params: { email: userMailById(userId) } }">
    <span>{{ userName }}</span>
  </router-link>
</template>

<script>
import { mapGetters } from "vuex";
export default {
    name: "UserLink",
    props: {
      userId: {
        type: String,
        required: true
      },
      userName: {
        type: String,
        required: true
      }
    },
    computed: 
    {
      ...mapGetters("users", ["userMailById"]),
    }
}
</script>


