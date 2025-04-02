FROM node:20-alpine

WORKDIR /frontend

COPY package.json .
RUN npm install

COPY . .

EXPOSE 80

RUN npm run build
# CMD ["npm", "run", "preview", "--", "--host", "0.0.0.0", "--port", "5173"]
CMD npm run preview