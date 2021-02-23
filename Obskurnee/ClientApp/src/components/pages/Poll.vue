<template>
<section>
  <h1 id="tableLabel">
    {{ poll.title }}<small v-if="poll.isClosed"> Uzavreté</small>
  </h1>

  <div v-if="poll.isClosed">
    <book-preview :book="{ bookId: poll.followupLink.entityId, post: poll.options.find(o => o.postId == poll.results.winnerPostId) }" 
      style="margin: auto;">Víťaz</book-preview>
    <router-link v-if="poll.followupLink?.kind == 'Discussion'" :to="{ name: 'discussion', params: { discussionId: poll.followupLink.entityId } }">
      <text-post :post="poll.options.find(o => o.postId == poll.results.winnerPostId)"  style="margin: auto;">Víťaz</text-post>
    </router-link>
  </div>
  <div v-if="!poll.isClosed && poll.results && poll.results.yetToVote" 
    style="margin:1em; border-color: pink; border-style:dashed;">
    Už hlasovalo {{ poll.results.alreadyVoted }} z {{ poll.results.totalVoters }}. <br />
    Ešte nehlasovala: <span v-for="person in poll.results.yetToVote" v-bind:key="person">{{ person }}</span>
  </div>
  <ol>
    <li v-for="option in poll.options" v-bind:key="option.postId">
      <input type="checkbox" :id="option.postId" :value="option.postId" v-model="checkedOptions" :disabled="iVoted"/>
      <label :for="option.postId" @click="toggleShow(option)"><strong>{{ option.title }}</strong> - {{ option.author }}</label>
    </li>
  </ol>
  <button @click="vote" :disabled="!checkedOptions?.length" class="btn btn-warning">Hlasuj!</button>
  
  <div v-if="iVoted && poll.results && poll.results.votes">
    <h2>VÝSLEDKY:</h2>
    <ol>
      <li v-for="vote in poll.results.votes" v-bind:key="vote">
         {{ poll.options.find(o => o.postId == vote.postId).title }} 
         - {{ vote.votes }} hlasy - {{ vote.percentage }}%
      </li>
    </ol>
  </div>

  <book-post v-if="previewId" v-bind:key="previewId.postId" v-bind:post="previewId" ></book-post>
  <a v-if="isMod" @click="closePoll(poll.pollId)" class="button">Zavri hlasovanie hned</a>
</section>
</template>


<script>
import BookPost from '../BookPost.vue';
import BookPreview from '../BookPreview.vue';
import TextPost from '../TextPost.vue';
import { mapGetters,mapActions, mapState } from "vuex";

export default {
  components: { BookPost, TextPost, BookPreview },
  name: "Poll",
  data() {
    return {
      previewId: null,
      checkedOptions: [],
      iVoted: false,
    };
  },
  computed: {
    ...mapGetters("context", ["isMod"]),
    ...mapState("polls", ["polls", "votes"]),
    poll: function () { return this.polls[this.$route.params.pollId] ?? { options: [] }; },
  },
  methods: {
    ...mapActions("polls", ["getPollData", "sendVote", "closePoll"]),
    toggleShow(postId){
      if (this.previewId == postId)
      {
        this.previewId = null;
      }
      else 
      {
        this.previewId = postId;
      }
    },
    vote()
    {
      if (!this.checkedOptions?.length)
      {
        return;
      }
      this.sendVote( { pollId: this.$route.params.pollId, votes: this.checkedOptions })
        .then((response) => {
          console.log('klientsky respons', response);
          this.iVoted = true;
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
  mounted() {
    this.getPollData(this.$route.params.pollId)
      .then(() => {
        this.checkedOptions = this.votes[this.$route.params.pollId] ?? [];
        if (this.checkedOptions?.length)
        {
          this.iVoted = true;
        }
      });
  },
};
</script>

<style>
</style>