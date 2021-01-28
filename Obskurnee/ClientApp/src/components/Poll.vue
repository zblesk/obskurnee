<template>
<section>
  <h1 id="tableLabel">
    {{ poll.title }}<small v-if="poll.isClosed"> Uzavreté</small>
  </h1>

  <div v-if="poll.isClosed">
  {{poll.followupLink}}
    <router-link v-if="poll.followupLink?.kind == 'Book'" :to="{ name: 'book', params: { bookId: poll.followupLink.entityId } }">
      <book-preview :post="poll.options.find(o => o.postId == poll.results.winnerPostId)"  style="margin: auto;">Víťaz</book-preview>
    </router-link>
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
  <button @click="vote" :disabled="!checkedOptions.length" class="btn btn-warning">Hlasuj!</button>
  
  <div v-if="iVoted && poll.results && poll.results.votes">
    <h2>VÝSLEDKY:</h2>
    <ol>
      <li v-for="vote in poll.results.votes" v-bind:key="vote">
         {{ poll.options.find(o => o.postId == vote.postId).title }} 
         - {{ vote.votes }} hlasy - {{ vote.percentage }}%
      </li>
    </ol>
  </div>

  <book-recommendation v-if="previewId" v-bind:key="previewId.postId" v-bind:post="previewId" ></book-recommendation>
</section>
</template>


<script>
import axios from "axios";
import BookRecommendation from './BookRecommendation.vue';
import BookPreview from './BookPreview.vue';
import TextPost from './TextPost.vue';

export default {
  components: { BookRecommendation, BookPreview, TextPost },
  name: "Poll",
  data() {
    return {
      poll: {},
      previewId: null,
      checkedOptions: [],
      iVoted: false,
    };
  },
  methods: {
    gePollData() {
      axios
        .get("/api/polls/" + this.$route.params.pollId)
        .then((response) => {
          console.log(response.data);
          this.poll = response.data.poll;
          this.poll.isClosed = true;
          if (response.data.myVote)
          {
             this.checkedOptions = response.data.myVote.postIds;
             this.iVoted = this.checkedOptions.length;
          }
        })
        .catch(function (error) {
          alert(error);
        });
    },
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
      if (!this.checkedOptions.length)
      {
        return;
      }
      axios.post(
          "/api/polls/" + this.$route.params.pollId + "/vote",
          { postIds: this.checkedOptions })
        .then((response) => {
          console.log(response.data);
          this.iVoted = true;
          this.poll = response.data.poll;
        })
        .catch(function (error) {
          alert(error);
        });
    },
    
    orderedResults() {
      let lst = [];
      return lst;
    }
  },
  computed: {
  },
  mounted() {
    this.gePollData();
  },
};
</script>

<style>
</style>