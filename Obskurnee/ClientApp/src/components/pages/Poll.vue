<template>
<section>
  <h1 id="tableLabel" class="page-title">
    {{ poll.title }}<span class="poll-closed" v-if="poll.isClosed">&nbsp;Uzavreté</span>
  </h1>
  <div v-if="poll.isClosed" class="main"> 
    <div v-if="poll.followupLink?.kind == 'Poll'">
      <h2 class="winner-title">Rozstrel</h2>
      <p class="tiebreaker-text">Máme několik vítězů se stejným počtem hlasů:</p>
      <ul class="tiebreaker-list">
        <li v-for="vote in poll.results.votes.filter(v => v.votes == poll.results.votes[0].votes)" v-bind:key="vote">{{ poll.options.find(o => o.postId == vote.postId).title }}</li>
      </ul>
      <router-link :to="{ name: 'poll', params: { pollId: poll.followupLink.entityId } }">
        <button class="button-primary">Přejít na rozstřelové hlasování</button>
      </router-link>
    </div>
    <div v-else>
      <h2 class="winner-title">Víťaz</h2>
      <book-preview v-if="poll.followupLink?.kind == 'Book'" 
        :book="{ bookId: poll.followupLink.entityId, post: poll.options.find(o => o.postId == poll.results.winnerPostId) }">
      </book-preview>
      <router-link v-if="poll.followupLink?.kind == 'Discussion'" 
        :to="{ name: 'discussion', params: { discussionId: poll.followupLink.entityId } }" class="link">
        <text-post :post="poll.options.find(o => o.postId == poll.results.winnerPostId)"></text-post>
      </router-link>
    </div>
  </div>

  <div class="page-wrapper flex">
    <div class="main flex">

      <div v-if="!poll.isClosed && poll.results && poll.results.alreadyVoted" class="u-md">
       <h2 class="subtitle">Stav hlasování</h2>
        <p class="paragraph">Už hlasovalo {{ poll.results.alreadyVoted.length }} z {{users.length }} čtenářů.</p>
        <p class="paragraph">Ješte nehlasovali: <span v-for="person in yetToVote" v-bind:key="person">{{ person.name }},</span></p>
      </div>

      <div v-if="iVoted || (poll.isClosed && poll.results && poll.results.votes)">
        <h2 class="subtitle">Výsledky</h2>
        <ol class="poll">
          <li class="poll-field" v-for="vote in poll.results.votes" v-bind:key="vote">
            {{ poll.options.find(o => o.postId == vote.postId).title }}:
             {{ vote.votes }} hlasy - {{ vote.percentage }}%
          </li>
        </ol>
      </div>

      <h2 v-if="poll.results" class="subtitle u-mt">Možnosti</h2>
      <ol class="poll">
        <li v-for="option in poll.options" v-bind:key="option.postId" class="poll-field">
          <input type="checkbox" :id="option.postId" :value="option.postId" v-model="checkedOptions" :disabled="iVoted || poll.isClosed" class="checkbox" />
          <label :for="option.postId" @click="toggleShow(option)"><span class="label-title">{{ option.title }}</span> <span v-if="option.author">({{ option.author }})</span></label>
        </li>
      </ol>

      <div class="buttons buttons-poll">
        <button @click="vote" :disabled="!checkedOptions?.length" class="button-primary" v-if="!iVoted && !poll.isClosed">Hlasovat</button>
        <button v-if="isMod && !poll.isClosed" @click="doClosePoll" class="button-secondary poll-to-close">Zavřít hlasování hned</button>
      </div>
      
    </div>
 
    <div class="book-post-wrapper flex">
      <div class="book-post-empty" v-if="!previewId">
        <div class="note note--poll">
          <div class="note-pic">
            <img src="../../assets/lamp.svg" alt="lamp icon" />
          </div>
          <p class="note-text">Klikni na název některé z možností v hlasování a zde se objeví podrobnější popis.</p>
        </div>
      </div>
      <book-post v-if="previewId" v-bind:key="previewId.postId" v-bind:post="previewId" ></book-post>
    </div> 

  </div> 

</section>
</template>

<script>
import { mapGetters,mapActions, mapState } from "vuex";
import BookPost from '../BookPost.vue';
import BookPreview from '../BookPreview.vue';
import TextPost from '../TextPost.vue';
export default {
  name: "Poll",
  components: { BookPost, TextPost, BookPreview },
  data() {
    return {
      previewId: null,
      checkedOptions: [],
      iVoted: false,
      poll: { options: [] }, 
    }; 
  },
  computed: {
    ...mapGetters("context", ["isMod"]),
    ...mapState("polls", ["polls", "votes"]),
    ...mapState("users", ["users"]),
    ...mapGetters("users", ["totalUsers"]),
    yetToVote: function() { return this.users.filter(user => !this.poll.results.alreadyVoted.some(u => u == user.userId)); }
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
      this.sendVote( { pollId: this.poll.pollId, votes: this.checkedOptions })
        .then(() => {
          this.poll = this.polls[this.poll.pollId];
          this.iVoted = true;
        })
        .catch((error) => this.$notifyError(error));
    },
    onLoad(newPollId)
    {
      if (!newPollId)
      {
        return;
      }
      this.getPollData(newPollId)
        .then(() => {
          this.poll = this.polls[newPollId];
          this.checkedOptions = this.votes[newPollId];
          this.iVoted = this.checkedOptions?.length > 0;
        });
    },
    doClosePoll()
    {
      this.closePoll(this.poll.pollId)
        .then((data) => {
          this.poll = data.poll;
        });
    }
  },
  watch: {
    '$route.params' ({ pollId }) {
      this.onLoad(pollId);
    }
  },
  mounted() {
    this.onLoad(this.$route.params.pollId);
  },
};
</script>

<style scoped>

  .poll-closed {
    font-size: 0.65em;
    color: var(--c-accent);
    text-transform: uppercase;
    vertical-align: super;
  }

  .poll-to-close {
    margin-top: var(--spacer);
  }

  @media screen and (min-width: 576px) {
    .poll-to-close {
      margin-top: 0;
      margin-left: var(--spacer);
    }
  }

  .subtitle {
    font-size: 1.25em;
    margin-top: 0;
    margin-bottom: calc(var(--spacer) / 2);
  }
  
  .u-mt {
    margin-top: calc(var(--spacer) * 2);
  }

  .u-md {
    margin-bottom: calc(var(--spacer) * 2);
  }

  .paragraph {
    margin-bottom: calc(var(--spacer) / 2);
  }

  .link {
    text-decoration: none;
  }

  .winner-title {
    text-align: center;
    font-size: 1.25em;
    margin-top: 0;
    margin-bottom: 0;
  }

  /* layout */
  .book-post-wrapper {
    max-width: 800px;
    margin: var(--spacer);
  }

  @media screen and (min-width: 840px) {
    .book-post-wrapper {
      margin: var(--spacer) auto;
    }
  }

  .note.note--poll {
    display: none;
  }

  @media screen and (min-width: 1200px) {
    .page-wrapper.flex {
      display: flex;
      max-width: 1660px;
      margin: 0 auto;
    }

    .book-post-wrapper.flex {
      flex: 1 1 50%;
      margin-left: var(--spacer);
      margin-right: var(--spacer);

      display: flex;
    }

    .main.flex {
      flex: 1 1 50%;
      margin-left: var(--spacer);
    }

    .book-post-empty {
      background-color: var(--c-bckgr-primary);
      width: 100%;
      height: 100%;
      padding: calc(var(--spacer) * 2);
    }

    .note.note--poll {
      display: flex;
    }
  }

  /* open poll */

  .poll {
    margin: 0 0 var(--spacer) 0;
    padding-left: calc(var(--spacer) * 2);
  }

  .poll-field:not(:last-child) {
    margin-bottom: calc(var(--spacer) / 2);
  }

  .poll-field input {
    margin-right: calc(var(--spacer) / 2);
    margin-left: calc(var(--spacer) / 2);
  }

  .checkbox:checked+label>.label-title {
    font-weight: bold;
  }

  @media screen and (min-width: 576px) {
    .buttons.buttons-poll {
      justify-content: flex-start;
    }
  }

  /* tiebreaker */

  .tiebreaker-text {
    margin-top: var(--spacer);
    margin-bottom: calc(var(--spacer) / 2);
  }

</style>
