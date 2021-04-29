<template>
<div>
  <div v-if="hide" class="button-show-wrapper">
    <button @click="toggleVisibility" class="button-primary button-show"><slot></slot></button>
  </div>
  <div v-else>
    <div class="form">
      <div class="cover">
        <img :src="newPost.imageUrl" v-if="newPost.imageUrl" :alt="newPost.title" />
      </div>
      <div class="form-text">
        <div v-if="mode != 'Themes'" class="form-field">
          <label for="grlink">{{$t('newpost.grLink')}}</label>
          <div class="note u-mb-sp05">
            <div class="note-pic">
                <img src="../assets/lamp.svg" alt="lamp icon" />
            </div>
            <p class="note-text">{{$t('newpost.grLoadInstructions')}}</p>
          </div>
          <input v-model="newPost.url" placeholder="https://www.goodreads.com/book..." @change="linkChange" id="grlink" />
          <p v-if="fetchInProgress" class="alert-inline">{{$t('newpost.waitForLoadPlox')}}</p>
        </div>
        <div class="cover-mobile">
          <img :src="newPost.imageUrl" v-if="newPost.imageUrl" :alt="newPost.title" />
        </div>
        <div class="form-field">
          <label for="name" v-if="mode == 'Books' || mode == 'Recommendations'">{{$t('newpost.bookTitle')}}*</label>
          <label for="name" v-if="mode == 'Themes'">{{$t('newpost.topicTitle')}}*</label>
          <input v-model="newPost.title" id="name" required />
        </div>
        <div class="form-field" v-if="mode != 'Themes'">
          <label for="author">{{$t('newpost.authorName')}}*</label>
          <input v-model="newPost.author" id="autor" required />
        </div>
        <div class="form-field" v-if="mode != 'Themes'">
          <label for="pages">{{$t('newpost.pageCount')}}*</label>
          <input type="number" v-model="newPost.pageCount" id="pages" required />
        </div>
        <div class="form-field u-mb-0">
          <div class="label-md-wrapper">
            <label for="text">{{$t('newpost.comment')}}*</label>
            <div class="mo-md">
              <div class="mo-md-pic">
                <img src="../assets/Markdown-mark.svg" alt="markdown logo">
              </div>
              <div class="mo-md-link">
                <markdown-help-link></markdown-help-link>
              </div>
            </div>
          </div>
          <textarea v-model="newPost.text" id="text" required :placeholder="$t('global.markdownSamplePlaceholder')"></textarea>
        </div>
        <p class="asterisk">{{$t('newpost.requiredFields')}}</p>
      </div>
    </div>
    <div class="buttons u-mt-sp">
      <button @click="addPost" :disabled="saveInProgress" class="button-primary">{{$t('menus.add')}}</button>
      <button @click="toggleVisibility" class="button-secondary hide-form">{{$t('newpost.hideForm')}}</button>
    </div>
  </div>
</div>
</template>

<style scoped>

  .button-show {
    display: block;
    width: 100%;
  }

  @media screen and (min-width: 576px) {
    .button-show-wrapper {
      display: flex;
      justify-content: center;
    }

    .button-show {
      display: inline-block;
      width: auto;
    }
  }

  .cover-mobile {
    max-width: 240px;
    margin: 0 auto var(--spacer) auto;
  }

  .cover {
    max-width: 240px;
    display: none;
  }

  .cover img,
  .cover-mobile img {
    width: 100%;
  }

  @media screen and (min-width: 840px) {
    .form {
      display: flex;
      flex-direction: row-reverse;
    }

    .form-text {
      flex-grow: 1;
    }

    .cover-mobile {
      display: none;
    }

    .cover {
      display: block;
      margin: 0 0 0 var(--spacer);
    }
  }

  .asterisk {
    font-size: 0.875em;
    opacity: 0.8;
    margin-top: calc(var(--spacer) /2);
  }

  .form-field.u-mb-0 {
    margin-bottom: 0;
  }

  .hide-form {
    margin-top: var(--spacer);
  }

  @media screen and (min-width: 576px) {
    .hide-form {
      margin-top: 0;
      margin-left: var(--spacer);
    }
  }



</style>

<script>
import axios from "axios";
import MarkdownHelpLink from "./MarkdownHelpLink.vue";
export default {
  name: "BookPost",
  components: { MarkdownHelpLink },
  props: {
    mode: {
      type: String,
      required: true,
      default: "",
    },
  },
  emits: ['new-post'],
  data() {
    return {
      newPost: {},
      fetchInProgress: false,
      saveInProgress: false,
      hide: true,
      parentData: null,
    };
  },
  methods: {
    addPost() {
      this.saveInProgress = true;
      this.$emit('new-post', 
        this.parentData
        ? { ... this.newPost, ... this.parentData}
        : this.newPost);
    },
    linkChange() {
      if (!this.newPost.url 
        || !this.newPost.url.startsWith("https://www.goodreads.com"))
      {
        return;
      }
      this.fetchInProgress = true;
      this.$notifyNormal("Nacitavam data");
        axios.get(
          "/api/scrape/?goodreadsUrl=" + this.newPost.url)
        .then((response) => {
          this.newPost = response.data;
          this.newPost.text = "\n\n### Popis na Goodreads: \n\n" + response.data.description;
          this.fetchInProgress = false;
          this.$notifyInfo("Nacitane");
        })
        .catch(function (error) {
          this.$notifyError(error);
          this.fetchInProgress = false;
        });
    },
    toggleVisibility()
    {
      this.hide = !this.hide;
    }
  },
  mounted() { 
    this.emitter.on("prefill-post", postData => {
      this.newPost = postData.post;
      this.parentData = postData.parentData;
      this.hide = false;
      this.saveInProgress = false;
    });
    this.emitter.on("clear-post", () => {
      this.newPost = {};
      this.parentData = null;
      this.hide = true;
      this.saveInProgress = false;
    });
  },
}
</script>
